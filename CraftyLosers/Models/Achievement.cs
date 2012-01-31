using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace CraftyLosers.Models
{
    public class Achievement
    {
        public int Id { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Qty/Duration")]
        public int Qty { get; set; }

        public int WorkoutRefId { get; set; }
    }
}