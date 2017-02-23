using System;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.PhantomJS;
using System.IO;
using System.Threading;
using OpenQA.Selenium.Internal;

namespace ClassLibrary1
{
    [TestFixture]
    public class PhantomjsTests
    {
        public static PhantomJSDriver _driver;

        [OneTimeSetUp]
        public void SetUp()
        {
            string dir = Path.GetFullPath(".");
            _driver = new PhantomJSDriver("C:\\app\\workspace\\submission\\evaltest\\HT-uitest\\PhantomJs.exe");
        }

        [Test, Property("Topic", "Working with Views and HTML Helpers")]
        public void RegistrationForm()
        {
            try
            {
                _driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(30));
                _driver.Navigate().GoToUrl("http://localhost:5000/Login/Register");

                IWebElement txtfname = _driver.FindElement(By.Id("Fname"));
                IWebElement txtlname = _driver.FindElement(By.Id("Lname"));
                IWebElement txtphone = _driver.FindElement(By.Id("Phone"));
                IWebElement txtmail = _driver.FindElement(By.Id("EmailID"));
                IWebElement txtaddress = _driver.FindElement(By.Id("Address"));
                IWebElement txtpass = _driver.FindElement(By.Id("Password"));
                IWebElement txtconpass = _driver.FindElement(By.Id("ConfirmPassword"));
                IWebElement chkadmin = _driver.FindElement(By.Id("IsAdmin"));
                Console.Out.WriteLine("Registration form existence checked");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
        }

        [Test, Property("Topic", "Validating User Input")]
        public void RegistrationRequiredCheck()
        {
            try
            {
                _driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(30));
                _driver.Navigate().GoToUrl("http://localhost:5000/Login/Register");

                var alltexts = _driver.FindElementsByCssSelector("[data-val='true']");
                System.Collections.Generic.List<string> controls = new System.Collections.Generic.List<string>();
                foreach (var item in alltexts)
                {
                    controls.Add(item.GetAttribute("Id"));
                }

                Assert.IsTrue(controls.Contains("Fname"));
                Assert.IsTrue(controls.Contains("Lname"));
                Assert.IsTrue(controls.Contains("Phone"));
                Assert.IsTrue(controls.Contains("EmailID"));
                Assert.IsTrue(controls.Contains("Password"));
                Assert.IsTrue(controls.Contains("ConfirmPassword"));
                Assert.IsTrue(controls.Contains("Address"));
                Console.Out.WriteLine("Required Field check is succeed");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
        }

        [Test, Property("Topic", "Validating User Input")]
        public void Registration_Email_Phone_type()
        {

            try
            {
                _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                _driver.Navigate().GoToUrl("http://localhost:5000/Login/Register");
                IWebElement txtphone = _driver.FindElement(By.Id("Phone"));
                IWebElement txtmail = _driver.FindElement(By.Id("EmailID"));

                Assert.AreEqual("tel", txtphone.GetAttribute("type"));
                Assert.AreEqual("email", txtmail.GetAttribute("type"));

                Console.Out.WriteLine("Phone and email fields are checked");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
        }

        [Test, Property("Topic", "Working with Views and HTML Helpers")]
        public void Login_NewRegistrationLink()
        {

            try
            {
                _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                _driver.Navigate().GoToUrl("http://localhost:5000/Login/Login");

                IWebElement btnregister = _driver.FindElement(By.Id("btnhtregisternew"));
                Assert.AreEqual("http://localhost:5000/Login/Register?mode=new", btnregister.GetAttribute("href"));
                Console.Out.WriteLine("New Registration link on login page is succeed");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
        }

        [Test, Property("Topic", "Validating User Input")]
        public void Register_Invalid_inputs()
        {

            try
            {
                _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                _driver.Navigate().GoToUrl("http://localhost:5000/Login/Register");

                IWebElement btnsubmit = _driver.FindElementById("btnhtregistration");
                btnsubmit.Submit();

                IWebElement fnameerror = _driver.FindElementById("Fname-error");
                IWebElement lnameerror = _driver.FindElementById("Lname-error");
                IWebElement phoneerror = _driver.FindElementById("Phone-error");
                IWebElement mailerror = _driver.FindElementById("EmailID-error");
                IWebElement passerror = _driver.FindElementById("Password-error");
                IWebElement conpasserror = _driver.FindElementById("ConfirmPassword-error");
                IWebElement addresserror = _driver.FindElementById("Address-error");

                Assert.AreEqual("First Name is required", fnameerror.GetAttribute("textContent"));
                Assert.AreEqual("Last Name is required", lnameerror.GetAttribute("textContent"));
                Assert.AreEqual("Email ID is required", mailerror.GetAttribute("textContent"));
                Assert.AreEqual("Password is required", passerror.GetAttribute("textContent"));
                Assert.AreEqual("Confirm Password is required", conpasserror.GetAttribute("textContent"));
                Assert.AreEqual("Mobile is required", phoneerror.GetAttribute("textContent"));
                Assert.AreEqual("Address is required", addresserror.GetAttribute("textContent"));
                Console.Out.WriteLine("Required field validation succeed");

                IWebElement txtphone = _driver.FindElement(By.Id("Phone"));
                IWebElement txtmail = _driver.FindElement(By.Id("EmailID"));

                txtphone.SendKeys("123456");
                txtmail.SendKeys("rohilla");
                txtmail.SendKeys(Keys.Tab);

                Assert.AreEqual("PhoneNumber should contain only numbers and must be 10 digits long", phoneerror.GetAttribute("textContent"));
                Assert.AreEqual("Please enter a valid email address", mailerror.GetAttribute("textContent"));
                Console.Out.WriteLine("Phone and email error check is succeed");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
        }

        [Test, Property("Topic", "Creating Controller with view")]
        public void Register_Clear()
        {
            try
            {
                _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                _driver.Navigate().GoToUrl("http://localhost:5000/Login/Register");

                IWebElement btnreset = _driver.FindElementById("btnhtclear");
                _driver.ExecuteScript("arguments[0].click();", btnreset);

                IWebElement txtfname = _driver.FindElement(By.Id("Fname"));
                IWebElement txtlname = _driver.FindElement(By.Id("Lname"));
                IWebElement txtphone = _driver.FindElement(By.Id("Phone"));
                IWebElement txtmail = _driver.FindElement(By.Id("EmailID"));
                IWebElement txtaddress = _driver.FindElement(By.Id("Address"));
                IWebElement txtpass = _driver.FindElement(By.Id("Password"));
                IWebElement txtconpass = _driver.FindElement(By.Id("ConfirmPassword"));
                IWebElement chkadmin = _driver.FindElement(By.Id("IsAdmin"));

                Assert.AreEqual(string.Empty, txtfname.GetAttribute("value"));
                Assert.AreEqual(string.Empty, txtlname.GetAttribute("value"));
                Assert.AreEqual(string.Empty, txtphone.GetAttribute("value"));
                Assert.AreEqual(string.Empty, txtmail.GetAttribute("value"));
                Assert.AreEqual(string.Empty, txtaddress.GetAttribute("value"));
                Assert.AreEqual(string.Empty, txtpass.GetAttribute("value"));
                Assert.AreEqual(string.Empty, txtconpass.GetAttribute("value"));
                Console.Out.WriteLine("Reset button functionality is succeed");
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
        }

        [Test, Property("Topic", "Creating Controller with view")]
        public void CheckAdminMenus_Options()
        {
            try
            {
                _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                _driver.Navigate().GoToUrl("http://localhost:5000/Login/Login");
                IWebElement txtmail = _driver.FindElement(By.Id("EmailID"));
                IWebElement txtpass = _driver.FindElement(By.Id("Password"));
                IWebElement btnsignin = _driver.FindElement(By.Id("btnhtsignin"));

                txtmail.SendKeys("admin@admin.com");
                txtpass.SendKeys("admin");
                btnsignin.Submit();

                Assert.AreEqual("http://localhost:5000/User/Admin", _driver.Url);
                Console.Out.WriteLine("Admin Login is checked");

                IWebElement lnkapprove = _driver.FindElement(By.LinkText("Approve"));
                Assert.IsTrue(lnkapprove.GetAttribute("href").Contains("Approve"));
                Console.Out.WriteLine("Admin page contains approve link");

                IWebElement lnkreject = _driver.FindElement(By.LinkText("Reject"));
                Assert.IsTrue(lnkreject.GetAttribute("href").Contains("Reject"));
                Console.Out.WriteLine("Admin page contains reject link");

                IWebElement btnuser = _driver.FindElementById("btnhtuserdetails");
                Assert.AreEqual("http://localhost:5000/User/UserDetails", btnuser.GetAttribute("href"));
                Console.Out.WriteLine("Admin page contains user details menu");

                IWebElement btnadmin = _driver.FindElementById("btnhtadmin");
                Assert.AreEqual("http://localhost:5000/User/Admin", btnadmin.GetAttribute("href"));
                Console.Out.WriteLine("Admin page contains Admin menu");

                IWebElement btnhome = _driver.FindElementById("btnhthome");
                Assert.AreEqual("http://localhost:5000/", btnhome.GetAttribute("href"));
                Console.Out.WriteLine("Admin page contains Home menu");

                IWebElement btnlogout = _driver.FindElementById("btnhtlogout");
                btnlogout.Submit();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
        }

        [Test, Property("Topic", "Creating Controller with view")]
        public void Admin_Userdetails_options()
        {
            try
            {
                _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                _driver.Navigate().GoToUrl("http://localhost:5000/Login/Login");
                IWebElement txtmail = _driver.FindElement(By.Id("EmailID"));
                IWebElement txtpass = _driver.FindElement(By.Id("Password"));
                IWebElement btnsignin = _driver.FindElement(By.Id("btnhtsignin"));

                txtmail.SendKeys("admin@admin.com");
                txtpass.SendKeys("admin");
                btnsignin.Submit();

                _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                IWebElement btnuser = _driver.FindElementById("btnhtuserdetails");

                _driver.ExecuteScript("arguments[0].click();", btnuser);

                IWebElement lnkedit = _driver.FindElement(By.LinkText("Edit"));
                Assert.IsTrue(lnkedit.GetAttribute("href").Contains("Edit"));
                Console.Out.WriteLine("Userdetails for Admin contains Edit option");

                IWebElement lnkdelete = _driver.FindElement(By.LinkText("Delete"));
                Assert.IsTrue(lnkdelete.GetAttribute("href").Contains("Delete"));
                Console.Out.WriteLine("Userdetails for Admin contains Delete option");

                IWebElement btnlogout = _driver.FindElementById("btnhtlogout");
                btnlogout.Submit();

            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
        }

        [Test, Property("Topic", "Creating Controller with view")]
        public void CheckNormalMenus_Options()
        {
            try
            {
                _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                _driver.Navigate().GoToUrl("http://localhost:5000/Login/Login");
                IWebElement txtmail = _driver.FindElement(By.Id("EmailID"));
                IWebElement txtpass = _driver.FindElement(By.Id("Password"));
                IWebElement btnsignin = _driver.FindElement(By.Id("btnhtsignin"));

                txtmail.SendKeys("rohilla@gmail.com");
                txtpass.SendKeys("1212");
                btnsignin.Submit();

                Assert.AreEqual("http://localhost:5000/User/Userdetails", _driver.Url);
                Console.Out.WriteLine("Normal user Login is checked");

                IWebElement btnuser = _driver.FindElementById("btnhtuserdetails");
                Assert.AreEqual("http://localhost:5000/User/UserDetails", btnuser.GetAttribute("href"));
                Console.Out.WriteLine("Normal user page contains user details menu");

                IWebElement btnhome = _driver.FindElementById("btnhthome");
                Assert.AreEqual("http://localhost:5000/", btnhome.GetAttribute("href"));
                Console.Out.WriteLine("Normal user page contains Home menu");

                IWebElement btnlogout = _driver.FindElementById("btnhtlogout");
                btnlogout.Submit();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
        }

        [Test, Property("Topic", "Creating Controller with view")]
        public void CheckNormal_Userdetails_Options()
        {
            //
            try
            {
                _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                _driver.Navigate().GoToUrl("http://localhost:5000/Login/Login");
                IWebElement txtmail = _driver.FindElement(By.Id("EmailID"));
                IWebElement txtpass = _driver.FindElement(By.Id("Password"));
                IWebElement btnsignin = _driver.FindElement(By.Id("btnhtsignin"));

                txtmail.SendKeys("rohilla@gmail.com");
                txtpass.SendKeys("1212");
                btnsignin.Submit();

                IWebElement lnkedit = _driver.FindElement(By.LinkText("Edit"));
                Assert.IsFalse(lnkedit.Displayed);
                Console.Out.WriteLine("Userdetails for Normal does not contain Edit option");

                IWebElement lnkdelete = _driver.FindElement(By.LinkText("Delete"));
                Assert.IsFalse(lnkdelete.Displayed);
                Console.Out.WriteLine("Userdetails for Normal does not contains Delete option");

                IWebElement btnlogout = _driver.FindElementById("btnhtlogout");
                btnlogout.Submit();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
        }

        [Test, Property("Topic", "Creating Controller with view")]
        public void Userdetails_searchbox()
        {
            try
            {
                _driver.Navigate().GoToUrl("http://localhost:5000/Login/Login");
                _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                IWebElement txtmail = _driver.FindElement(By.Id("EmailID"));
                IWebElement txtpass = _driver.FindElement(By.Id("Password"));
                IWebElement btnsignin = _driver.FindElement(By.Id("btnhtsignin"));

                txtmail.SendKeys("rohilla@gmail.com");
                txtpass.SendKeys("1212");
                btnsignin.Submit();

                IWebElement txtsearch = _driver.FindElementById("txtSearch");
                IWebElement btnsearch = _driver.FindElementById("btnSearch");

                Assert.AreEqual("Search", btnsearch.GetAttribute("value"));
                Assert.AreEqual("text", txtsearch.GetAttribute("type"));
                Console.Out.WriteLine("Userdetails contains a textbox and button to search");

                txtsearch.SendKeys("zzzzz");
                btnsearch.Submit();

                IWebElement lblerr = _driver.FindElementById("lblmesg");
                Assert.AreEqual("No records found", lblerr.GetAttribute("textContent"));
                Console.Out.WriteLine("Error label for search box is succeed");

                IWebElement btnlogout = _driver.FindElementById("btnhtlogout");
                btnlogout.Submit();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Thread.Sleep(10000);
            _driver.Quit();
        }
    }
}
