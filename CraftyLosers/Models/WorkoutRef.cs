using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CraftyLosers.Models
{
    public class WorkoutRef
    {
        [DisplayName("Workout Reference Id")]
        public int Id { get; set; }

        [DisplayName("Workout Description")]
        public string Description { get; set; }

        [DisplayName("Calories Per Minute/Rep")]
        public decimal Calories { get; set; }

        [DisplayName("Is Rep")]
        public bool Reps { get; set; }
    }
}