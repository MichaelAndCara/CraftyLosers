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
using CraftyLosers.Util;

namespace CraftyLosers.Models
{
    public class User
    {
        [DisplayName("User Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Username")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "User name must not be less than 3 or exceed 20 characters.")]
        public string UserName { get; set; }

        [DisplayName("Display Username")]
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
        public decimal? StartWeight { get; set; }

        [DisplayName("Goal Weight")]
        [Range(typeof(Decimal), "80", "500", ErrorMessage = "Weight must be between 80.00 and 500.00")]
        public decimal? GoalWeight { get; set; }

        [DisplayName("End Weight")]
        [Range(typeof(Decimal), "80", "500", ErrorMessage = "Weight must be between 80.00 and 500.00")]
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
        public ICollection<WorkoutLog> WorkoutLogs { get; set; }
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
}