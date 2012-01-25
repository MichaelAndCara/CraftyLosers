using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using CraftyLosers.Repositories;
using System.ComponentModel.DataAnnotations;

namespace CraftyLosers.ViewModels
{
    public class Workout
    {
        CraftyContext db = new CraftyContext();

        public Workout()
        {
            //var dw = db.WorkoutRefs.Where(e => e.Reps == false);
            DurationWorkouts = new SelectList(db.WorkoutRefs.Where(e => e.Reps == false), "Id", "Description");
            RepWorkouts = new SelectList(db.WorkoutRefs.Where(e => e.Reps == true), "Id", "Description");
        }

        public int DurationId { get; set; }

        [Required]
        [DisplayName("Workout Date")]
        [DataType(DataType.Date)]
        public DateTime? WorkoutDate { get; set; }

        [DisplayName("Duration Workouts")]
        public SelectList DurationWorkouts { get; set; }
        
        [DisplayName("Duration in Minutes")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Must be a whole positive number")]
        [Range(0,480, ErrorMessage="Duration must be between 0 and 480")]
        public int? Duration { get; set; }

        public int RepId { get; set; }

        [DisplayName("Rep Workouts")]
        public SelectList RepWorkouts { get; set; }
        
        [DisplayName("Reps")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Must be a whole positive number")]
        [Range(0, 480, ErrorMessage = "Reps must be between 0 and 480")]
        public int? Rep { get; set; }
    }
}