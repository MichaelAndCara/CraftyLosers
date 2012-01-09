using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Globalization;
using CraftyLosers.Models;
using System.Web.Security;
using System.Net.Mail;

namespace CraftyLosers.ViewModels
{
    public class Register
    {
        public User User { get; set; }

        //[Required]
        //[DisplayName("User name")]
        //[StringLength(20, MinimumLength = 3, ErrorMessage = "User name must not be less than 3 or exceed 20 characters.")]
        //public string UserName { get; set; }

        //[Required]
        //[StringLength(20, MinimumLength = 3, ErrorMessage = "Password name must not be less than 3 or exceed 20 characters.")]
        //[DataType(DataType.Password)]
        //[DisplayName("Password")]
        //public string Password { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Password name must not be less than 3 or exceed 20 characters.")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [ValidateEmail]
        public string Email { get; set; }

        //public bool Active { get; set; }
        //public bool Admin { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateEmailAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "Invalid Email";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidateEmailAttribute() : base(_defaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            string valueAsString = value == null ? "meh" : value as string;
            try
            {
                MailAddress m = new MailAddress(valueAsString);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}