using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace CraftyLosers.Models
{
    public class User
    {
        [DisplayName("User Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("User name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "User name must not be less than 3 or exceed 20 characters.")]
        public string UserName { get; set; }

        [DisplayName("Display User Name")]
        public bool DisplayName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        //[StringLength(20, MinimumLength = 3, ErrorMessage = "Password name must not be less than 3 or exceed 20 characters.")]
        public string PW { get; set; }

        public bool Active { get; set; }

        public DateTime SignUpDateTime { get; set; }

        [DisplayName("Start Weight")]
        [Range(typeof(Decimal), "80", "500", ErrorMessage = "Weight must be between 80.00 and 500.00")]
        [RegularExpression(@"[0-9]*\.?[0-9]+")]
        public decimal? StartWeight { get; set; }

        [DisplayName("Goal Weight")]
        [Range(typeof(Decimal), "80", "500", ErrorMessage = "Weight must be between 80.00 and 500.00")]
        [RegularExpression(@"[0-9]*\.?[0-9]+")]
        public decimal? GoalWeight { get; set; }

        [DisplayName("End Weight")]
        [Range(typeof(Decimal), "80", "500", ErrorMessage = "Weight must be between 80.00 and 500.00")]
        [RegularExpression(@"[0-9]*\.?[0-9]+")]
        public decimal? EndWeight { get; set; }

        public bool Admin { get; set; }

        [DisplayName("Paid")]
        [DataType(DataType.Currency)]
        public decimal? Paid { get; set; }

        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public ICollection<WeightCheckIn> WeightCheckIns { get; set; }
        public ICollection<Post> Posts { get; set; }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) 
                throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public class AccountEmailManager
    {
        public void EmailRegister(User user)
        {
            var fromAddress = new MailAddress("craftyloser@gmail.com");
            var toAddress = new MailAddress(user.Email, user.UserName);
            const string fromPassword = "pr3v3n1n9";
            const string subject = "Welcome to Crafty Losers!";
            string body = "Welcome to Crafty Losers!  Good luck!";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
            {
                smtp.Send(message);
            }
        }
    }
}