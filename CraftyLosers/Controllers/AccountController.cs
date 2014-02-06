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
using CraftyLosers.Util;
using System.Web.Security;
using System.Data.Entity;

namespace CraftyLosers.Controllers
{
    
    [HandleError]
    public class AccountController : Controller
    {
        CraftyContext db = new CraftyContext();

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
                
                var user = db.Users.Where(e => e.UserName == register.User.UserName).FirstOrDefault();

                if (user != null)
                {
                    ModelState.AddModelError("", "Username already exists");
                    return View(register);
                }

                var userEmail = db.Users.Where(e => e.Email == register.Email).FirstOrDefault();

                if (userEmail != null)
                {
                    ModelState.AddModelError("", "Email already exists");
                    return View(register);
                }

                register.User.PW = Convert.ToBase64String(
                    new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(
                        Encoding.ASCII.GetBytes(register.ConfirmPassword)));
                register.User.SignUpDateTime = DateTime.Now;
                register.User.Email = register.Email;
                register.User.Active = true;

                var newUser = register.User;

                db.Users.Add(newUser);
                db.SaveChanges();

                using (var mySmtp = new MySmtpClient())
                {
                    using (var message = new MyEmail(newUser.Email))
                    {
                        message.Subject = "Welcome to Crafty Losers!";
                        message.Body = "Welcome to Crafty Losers!  Good luck!";
                        mySmtp.Send(message);
                    }
                }

                IFormsAuthenticationService formsService = new FormsAuthenticationService();
                    formsService.SignIn(newUser.UserName, true);

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

                var contextUser = db.Users.Where(e => e.UserName == logOn.User.UserName && e.PW == logOn.User.PW).FirstOrDefault();

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

        [Authorize]
        public ActionResult Account()
        {
            return View(db.Users.Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Account(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Account");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = db.Users.Where(e => e.UserName == userName).FirstOrDefault();
            if (user != null)
            {
                var changePassword = new ChangePassword()
                {
                    Id = user.Id
                };
                return View(changePassword);
            }
            return RedirectToAction("LogOn");
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            if (!String.Equals(changePassword.NewPW, changePassword.ConfirmNewPW))
            {
                ModelState.AddModelError("", "Password and confirm password does not match");
                return View(changePassword);
            }

            string origPW = Convert.ToBase64String(
                new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(
                    Encoding.ASCII.GetBytes(changePassword.PW)));

            var user = db.Users.Where(e => e.Id == changePassword.Id &&
                e.UserName == HttpContext.User.Identity.Name && 
                e.PW == origPW).FirstOrDefault();
                
            if (user != null)
            {
                user.PW = Convert.ToBase64String(
                new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(
                    Encoding.ASCII.GetBytes(changePassword.NewPW)));

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("", "The password provided is incorrect.");
                return View(changePassword);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ForgotUserNameOrPassword forgotUserNameOrPassword)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(e => e.Email == forgotUserNameOrPassword.Email).FirstOrDefault();

                if (user != null)
                {
                    string newPassword = Guid.NewGuid().ToString().Split('-')[4];
                    user.PW = Convert.ToBase64String(
                        new System.Security.Cryptography.SHA1CryptoServiceProvider().ComputeHash(
                            Encoding.ASCII.GetBytes(newPassword)));

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    using (var mySmtp = new MySmtpClient())
                    {
                        using (var message = new MyEmail(user.Email))
                        {
                            message.Subject = "Crafty Loser Password resest notification";
                            message.Body = "New password is " + newPassword;
                            mySmtp.Send(message);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found.");
                    return View(forgotUserNameOrPassword);
                }
            }
            else
            {
                return View(forgotUserNameOrPassword);
            }

            return RedirectToAction("ResetPasswordNotification");
        }

        public ActionResult ResetPasswordNotification()
        {
            ViewBag.Notify = "New password sent to your email";
            return View();
        }

        public ActionResult ForgotUsername()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotUsername(ForgotUserNameOrPassword forgotUserNameOrPassword)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(e => e.Email == forgotUserNameOrPassword.Email).FirstOrDefault();

                if (user != null)
                {
                    using (var mySmtp = new MySmtpClient())
                    {
                        using (var message = new MyEmail(user.Email))
                        {
                            message.Subject = "Crafty Loser username request";
                            message.Body = "Username is " + user.UserName;
                            mySmtp.Send(message);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found.");
                    return View(forgotUserNameOrPassword);
                }
            }
            else
            {
                return View(forgotUserNameOrPassword);
            }

            return RedirectToAction("ForgotUsernameNotification");
        }

        public ActionResult ForgotUsernameNotification()
        {
            ViewBag.Notify = "Username sent to your email";
            return View();
        }

        public ActionResult AdminWeights()
        {
            return View(db.Users);
        }

        public ActionResult AdminWeightEdit(int id)
        {
            return View(db.Users.Find(id));
        }

        [HttpPost]
        public ActionResult AdminWeightEdit(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AdminWeights");
        }
    }
}
