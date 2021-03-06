﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CraftyLosers.Models
{
    public class WeightCheckIn
    {
        [DisplayName("Weight CheckIn Id")]
        public int Id { get; set; }

        [DisplayName("User Id")]
        public int UserId { get; set; }

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