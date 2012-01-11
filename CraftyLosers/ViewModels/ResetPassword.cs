using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CraftyLosers.ViewModels
{
    public class ResetPassword
    {
        [Required]
        [DisplayName("User name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "User name must not be less than 3 or exceed 20 characters.")]
        public string UserName { get; set; }

        //[DisplayName("Email")]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }
    }
}