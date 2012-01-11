using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CraftyLosers.ViewModels
{
    public class ForgotUserNameOrPassword
    {
        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [ValidateEmail]
        public string Email { get; set; }
    }
}