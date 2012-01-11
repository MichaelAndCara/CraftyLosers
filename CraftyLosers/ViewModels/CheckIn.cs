using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CraftyLosers.ViewModels
{
    public class CheckIn
    {
        [Required]
        [DisplayName("CheckIn Date")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DisplayName("Weight")]
        [Range(typeof(Decimal), "80", "500", ErrorMessage = "Weight must be between 80.00 and 500.00")]
        public decimal Weight { get; set; }
    }
}