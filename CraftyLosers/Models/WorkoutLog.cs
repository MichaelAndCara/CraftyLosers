using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CraftyLosers.Models
{
    public class WorkoutLog
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int  WorkoutRefId { get; set; }

        [DisplayName("Workout Date")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime WorkoutDate { get; set; }

        [DisplayName("Qty/Duration")]
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Must be a whole positive number")]
        [Range(1, 480, ErrorMessage = "must be between 1 and 480")]
        public int Qty { get; set; }

        [DisplayName("Calories")]
        public decimal Calories { get; set; }

        public User User { get; set; }
        public WorkoutRef WorkoutRef { get; set; }
    }
}