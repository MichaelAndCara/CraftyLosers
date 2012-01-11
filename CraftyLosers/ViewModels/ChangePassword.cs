using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CraftyLosers.ViewModels
{
    public class ChangePassword
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Password name must not be less than 3 or exceed 20 characters.")]
        [DataType(DataType.Password)]
        [DisplayName("Current password")]
        public string PW { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Password name must not be less than 3 or exceed 20 characters.")]
        [DataType(DataType.Password)]
        [DisplayName("New password")]
        public string NewPW { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Password name must not be less than 3 or exceed 20 characters.")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm new password")]
        public string ConfirmNewPW { get; set; }
    }
}