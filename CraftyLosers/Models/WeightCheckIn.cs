using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CraftyLosers.Models
{
    public class WeightCheckIn
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CheckInDate { get; set; }
        public decimal Weight { get; set; }
    }
}