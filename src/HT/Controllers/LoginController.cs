using HT.Models;
using HT.UserViewModel;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HT.Controllers
{
    public class LoginController : Controller
    {
        private static int uid;
        private HarmonyToysDatabaseContext context;

        public LoginController(HarmonyToysDatabaseContext _context)
        {
            context = _context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var role = string.Empty;
            if (ModelState.IsValid)
            {
                TblLoginDetails user = ChkLoginCredentials(model);
                if (user != null)
                {
                    ViewData["msg"] = "Valid User";

                    role = await LoginRedirect(user);

                    if (role == "Admin")
                        return RedirectToAction("Admin", "User");
                    else
                        return RedirectToAction("Userdetails", "User");
                }
                else
                {
                    ViewData["msg"] = "InValid User";
                }
            }
            ModelState.Clear();
            return View("Login");
        }


        [HttpGet]
        public IActionResult Register(int id, string mode)
        {
            if (mode == "edit")
            {
                Registration regdetails = Showdata(id);
                ViewData["mode"] = "edit";
                uid = id;
                return View("Registration", regdetails);
            }
            ViewData["mode"] = "new";
            return View("Registration");
        }

        public Registration Showdata(int id)
        {
            return (from logins in context.TblLoginDetails
                    join users in context.TblUserDetails
                    on logins.UserId equals users.UserId
                    where users.UserId == id
                    select new Registration { Fname = users.Fname, Lname = users.Lname, EmailID = users.EmailId, Phone = users.Phone.ToString(), Address = users.Address, IsAdmin = (logins.IsAdmin == "Y" ? true : false) }).SingleOrDefault();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Registration model, string mode)
        {
            ViewData["mode"] = string.Empty;
            if (ModelState.IsValid)
            {
                if (mode == "update")
                {
                    UpdateUser(uid, model);
                    return RedirectToAction("Admin", "User");
                }
                else
                {
                    CreateUser(model);
                    TblLoginDetails user = new TblLoginDetails { UserName = model.EmailID, Password = model.Password, IsAdmin = model.IsAdmin ? "Y" : "N", IsApproved="N" };
                    var role = await LoginRedirect(user);
                    if (role == "Admin")
                    {
                        return RedirectToAction("Admin", "User");
                    }
                    else
                    {
                        return RedirectToAction("Userdetails", "User");
                    }
                }
            }

            return View("Registration");
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.Authentication.SignOutAsync("MyCookieMiddlewareInstance");
            return RedirectToActionPermanent("Index", "Home");
        }

        public IActionResult Clear()
        {
            ModelState.Clear();
            return RedirectToAction("Register");
        }

        [NonAction]
        public void CreateUser(Registration model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                TblUserDetails user = new TblUserDetails { Fname = model.Fname, Lname = model.Lname, EmailId = model.EmailID, Phone = Convert.ToDecimal(model.Phone), Address = model.Address };
                context.TblUserDetails.Add(user);
                context.SaveChanges();

                string adminchk = model.IsAdmin ? "Y" : "N";

                TblLoginDetails login = new TblLoginDetails { UserName = model.EmailID, Password = model.Password, IsAdmin = adminchk, UserId = user.UserId };
                context.TblLoginDetails.Add(login);
                context.SaveChanges();
                transaction.Commit();
            }
        }

        [NonAction]
        public void UpdateUser(int id, Registration model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                TblUserDetails user = context.TblUserDetails.Where(u => u.UserId == id).FirstOrDefault();
                user.Fname = model.Fname;
                user.Lname = model.Lname;
                user.EmailId = model.EmailID;
                user.Phone = Convert.ToDecimal(model.Phone);
                user.Address = model.Address;

                context.SaveChanges();

                string adminchk = model.IsAdmin ? "Y" : "N";

                TblLoginDetails login = context.TblLoginDetails.Where(u => u.UserId == id).FirstOrDefault();
                login.UserName = model.EmailID;
                login.Password = model.Password;
                login.IsAdmin = adminchk;

                context.SaveChanges();
                transaction.Commit();
            }
        }

        private TblLoginDetails ChkLoginCredentials(LoginViewModel model)
        {
            return context.TblLoginDetails.Where(u => u.UserName == model.EmailID && u.Password == model.Password).SingleOrDefault();
        }

        private async Task<string> LoginRedirect(TblLoginDetails user)
        {
            string role;
            if (user.IsAdmin == "Y")
            {
                role = "Admin";
            }
            else
            {
                role = "NormalUser";
            }

            var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Role,role)
                    };

            var userIdentity = new ClaimsIdentity(claims, "Passport");
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.Authentication.SignInAsync("MyCookieMiddlewareInstance", userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                IsPersistent = false,
                AllowRefresh = false
            }
             );
            return role;
        }
    }
}