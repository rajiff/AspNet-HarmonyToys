using HT.Controllers;
using HT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using HT.UserViewModel;
using System.Security.Claims;
using Moq;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace HT_Test
{
    [TestFixture]
    public class ControllerTest
    {
        private UserController usercontroller;
        private LoginController controller;
        public static IConfigurationRoot Config { get; private set; }

        private static void SetupSimpleConfiguration()
        {
            Config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        }

        public ControllerTest()
        {
            SetupSimpleConfiguration();
            string constr = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");
            if (constr == null)
            {
                constr= Config["DefaultConnection"]; ;
            }
            var optionsBuilder = new DbContextOptionsBuilder<HarmonyToysDatabaseContext>();
            optionsBuilder.UseSqlServer(constr);
            var context = new HarmonyToysDatabaseContext(optionsBuilder.Options);

            usercontroller = new UserController(context);   
            controller = new LoginController(context);
        }

        [Test,Property("Creating Controller with view","Checks ViewResult")]
        public async Task Admin_Returns_ViewResult()
        {
            var result =await usercontroller.Admin() as ViewResult;
            Console.Out.WriteLine("Phone Number {0}",((List<UserApproval>)result.Model).FirstOrDefault().Mobile);

            Assert.IsTrue(result.ViewName == "Admin");
        }

        [Test,Property("Creating Controller with view", "Checks View and Model")]
        public void Userdetails_search_ViewAndModel()
        {
            string txtsearch = "a";
            var result = usercontroller.Userdetails(txtsearch) as ViewResult;
            
            Assert.IsTrue(result.ViewName == "Userdetails");
            Assert.IsInstanceOf(typeof(List<TblUserDetails>), result.Model);
        }

        [Test,Property("Routing & Friendly URLs", "Programming Redirection")]
        public void Userdetails_Edit_RedirectToRegister()
        {
            int userid = 2;
            var result = usercontroller.Edit(userid) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "Register");
            Assert.IsTrue(result.ControllerName == "Login");
            Assert.AreEqual(userid, result.RouteValues["id"]);
        }
        
        [Test,Property("Working with Data", "Displaying and Updating Data")]
        public async Task Userdetails_Admin_Approve()
        {
            int id = 3;
            await usercontroller.Approve(id);
            Assert.AreEqual("Approved", usercontroller.userstatus.Where(u => u.loginid == id).FirstOrDefault().Status);
        }

        [Test, Property("Working with Data", "Displaying and Updating Data")]
        public async Task Userdetails_Admin_Reject()
        {
            int id = 3;
            await usercontroller.Reject(id);
            Assert.AreEqual("Rejected", usercontroller.userstatus.Where(u => u.loginid == id).FirstOrDefault().Status);
        }

        [Test,Property("Middleware", "Custom Middleware")]
        public async Task Login_ReturnsAViewResultWithAdmin()
        {
            LoginViewModel loginVmodel = new LoginViewModel { EmailID = "admin@admin.com", Password = "admin" };

            var claimPrincipal = new ClaimsPrincipal();

            var mockAuth = new Mock<AuthenticationManager>();
            mockAuth.Setup(c => c.SignInAsync("MyCookieMiddlewareInstance", claimPrincipal)).Returns(Task.FromResult("done"));

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.Authentication).Returns(mockAuth.Object);

            var fakeActionContext = new ActionContext(mockContext.Object, new RouteData(), new ControllerActionDescriptor());
            var contContext = new ControllerContext(fakeActionContext);

            controller.ControllerContext = contContext;

            var result = await controller.Login(loginVmodel) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "Admin" && result.ControllerName == "User");

        }

        [Test, Property("Middleware", "Custom Middleware")]
        public async Task Login_Returns_Userdetails()
        {
            LoginViewModel loginVmodel = new LoginViewModel { EmailID = "rohilla@gmail.com", Password = "1212" };

            var claimPrincipal = new ClaimsPrincipal();

            var mockAuth = new Mock<AuthenticationManager>();
            mockAuth.Setup(c => c.SignInAsync("MyCookieMiddlewareInstance", claimPrincipal)).Returns(Task.FromResult("done"));

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.Authentication).Returns(mockAuth.Object);

            var fakeActionContext = new ActionContext(mockContext.Object, new RouteData(), new ControllerActionDescriptor());
            var contContext = new ControllerContext(fakeActionContext);

            controller.ControllerContext = contContext;

            var result = await controller.Login(loginVmodel) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "Userdetails" && result.ControllerName == "User");
        }

        [Test, Property("Security", "Authentication and Authorization")]

        public async Task Login_Invalid()
        {
            LoginViewModel loginVmodel = new LoginViewModel { EmailID = "rohilla@gmail.com", Password = "121" };
            var result = await controller.Login(loginVmodel) as ViewResult;

            Assert.IsTrue(result.ViewName == "Login");
            Assert.AreEqual("InValid User", result.ViewData["msg"].ToString());
        }

        [Test, Property("Routing & Friendly URLs", "Programming Redirection")]
        public async Task Register_Admin_Redirect()
        {
            Registration user = new Registration { Fname = "Ashish", Lname = "Mittal", Address = "Delhi", Phone = "9876543210", EmailID = "ash@gmail.com", Password = "abcd", ConfirmPassword = "abcd", IsAdmin = true };
            string operation = "new";

            var result = await controller.Register(user, operation) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "Admin" && result.ControllerName == "User");
        }

        [Test, Property("Routing & Friendly URLs", "Programming Redirection")]
        public async Task Register_Normaluser_Redirect()
        {
            Registration user = new Registration { Fname = "Sachin", Lname = "Garg", Address = "Delhi", Phone = "9212345670", EmailID = "sachin@gmail.com", Password = "abcd", ConfirmPassword = "abcd", IsAdmin = false };
            string operation = "new";

            var result = await controller.Register(user, operation) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "Userdetails" && result.ControllerName == "User");
        }
      
    }
}
