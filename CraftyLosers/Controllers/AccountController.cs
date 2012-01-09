using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CraftyLosers.ViewModels;
using CraftyLosers.Repositories;
using System.Text;
using System.Data;
using CraftyLosers.Models;
using System.Net.Mail;
using System.Net;

namespace CraftyLosers.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View(new Register());
        }

        [HttpPost]
        public ActionResult Register(Register register)
        {
            if (ModelState.IsValid)
            {

                if (!String.Equals(register.User.PW, register.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Password and confirm password does not match");
                    return View(register);
                }

                using (var db = new CraftyContext())
                {
                    var user = db.Users.Where(e => e.UserName == register.User.UserName).FirstOrDefault();

                    if (user != null)
                    {
                        ModelState.AddModelError("", "Username already exists");
                        return View(register);
                    }

                    register.User.PW = Convert.ToBase64String(
                        new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(
                            Encoding.ASCII.GetBytes(register.ConfirmPassword)));
                    register.User.SignUpDateTime = DateTime.Now;
                    register.User.Email = register.Email;

                    var newUser = register.User;

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    var emailManager = new AccountEmailManager();
                    emailManager.EmailRegister(newUser);

                    IFormsAuthenticationService formsService = new FormsAuthenticationService();
                        formsService.SignIn(newUser.UserName, true);

                    //return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return View(register);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOn logOn, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                logOn.User.PW = Convert.ToBase64String(
                    new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(
                        Encoding.ASCII.GetBytes(logOn.User.PW)));

                using (var db = new CraftyContext())
                {
                    var contextUser = db.Users.Where(e => e.UserName.ToLower() == logOn.User.UserName.ToLower() && e.PW == logOn.User.PW).FirstOrDefault();

                    if (contextUser != null)
                    {
                        IFormsAuthenticationService formsService = new FormsAuthenticationService();
                        formsService.SignIn(logOn.User.UserName, logOn.RememberMe);

                        if (!String.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            //return View(new LogOnViewModel(logOnModel));

            return View(new LogOn());
        }

        public ActionResult LogOff()
        {
            IFormsAuthenticationService formsService = new FormsAuthenticationService();
            formsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Account()
        {
            string userName = HttpContext.User.Identity.Name;
            using (var db = new CraftyContext())
            {
                var user = db.Users.Where(e => e.UserName == userName).FirstOrDefault();
                if (user != null)
                {
                    return View(user);
                }
            }
            return RedirectToAction("LogOn");
            //return View(AccountRepository.GetAccount(user));
        }

        [HttpPost]
        public ActionResult Account(User user)
        {
            using (var db = new CraftyContext())
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Account");
        }
    }
}
