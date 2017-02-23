using HT.Models;
using HT.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HT.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
       private HarmonyToysDatabaseContext context;
       public List<UserApproval> userstatus = null;

        public UserController(HarmonyToysDatabaseContext _context)
        {
            context = _context;
        }

        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Admin()
        {
            if(userstatus==null)
            {
                userstatus = await this.RetrieveApprovalStatus();
            }
            
            return View("Admin", userstatus);
        }

        public async Task<IActionResult> Approve(int id)
        {
            TblLoginDetails login = context.TblLoginDetails.Find(id);
            login.IsApproved = "Y";
            context.SaveChanges();

            userstatus = await this.RetrieveApprovalStatus();
            return RedirectToAction("Admin");
        }

        public async Task<IActionResult> Reject(int id)
        {
            TblLoginDetails login = context.TblLoginDetails.Find(id);
            login.IsApproved = "N";
            context.SaveChanges();

            userstatus = await this.RetrieveApprovalStatus();
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public IActionResult Userdetails(string txtSearch)
        {
            List<TblUserDetails> users = RetreiveSearchDetails(txtSearch);
            return View("Userdetails", users);
        }

        public List<TblUserDetails> RetreiveSearchDetails(string txtSearch)
        {
            if (txtSearch == null)
            {
                return context.TblUserDetails.ToList();
            }
            else
            {
                List<TblUserDetails> data=context.TblUserDetails.Where(u => u.Fname.StartsWith(txtSearch)).ToList();
                ViewBag.Records = data.Count;
                return data;
            }
        }

        public void M1()
        {
            
        }

        public IActionResult Delete(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                var userdetail = context.TblUserDetails.Where(l => l.UserId == id).SingleOrDefault();
                context.TblUserDetails.Remove(userdetail);
                context.SaveChanges();
                transaction.Commit();
            }

            return RedirectToAction("Userdetails");
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                var userdetail = context.TblUserDetails.Where(l => l.UserId == id).SingleOrDefault();
                context.TblUserDetails.Remove(userdetail);
                context.SaveChanges();
                transaction.Commit();
            }

            return RedirectToAction("Userdetails");
        }

        public IActionResult Edit(int id)
        {
            return RedirectToAction("Register", "Login", new { id = id, mode = "edit" });
        }

        private async Task<List<UserApproval>> RetrieveApprovalStatus()
        {
            return await (from logins in context.TblLoginDetails
                    join users in context.TblUserDetails
                    on logins.UserName equals users.EmailId
                    select new UserApproval { UserName = logins.UserName, Mobile = Convert.ToUInt64(users.Phone).ToString(), Status = (logins.IsApproved == "Y" ? "Approved" : "Rejected"), loginid = logins.LoginId }).ToListAsync();
        }

        private void ApproveOrReject(int id, string operation)
        {
            TblLoginDetails login = context.TblLoginDetails.Find(id);
            if (operation == "approve" && login.IsApproved == "N")
            {
                login.IsApproved = "Y";
            }
            else if (operation == "reject" && login.IsApproved == "Y")
            {
                login.IsApproved = "N";
            }

            context.SaveChanges();
        }
    }
}