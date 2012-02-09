using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using CraftyLosers.Models;
using System.ComponentModel.DataAnnotations;

namespace CraftyLosers.ViewModels
{
    public class Stats
    {
        public Stats(User user)
        {
            StartingWeight = Convert.ToDecimal(user.StartWeight);
            GoalWeight = Convert.ToDecimal(user.GoalWeight);
            CurrentWeight = user.WeightCheckIns.OrderByDescending(e => e.CheckInDate).ToList()[0].Weight;
            User = user;
        }

        public User User { get; private set; }

        [DisplayName("Total days left of the competition")]
        public int DaysLeft { get { return (new DateTime(2012, 4, 27) - DateTime.Today).Days; } }

        [DisplayName("Total weeks left of the competition")]
        public int WeeksLeft { get { return Convert.ToInt32(Math.Ceiling((double)DaysLeft / 7)); } }

        [DisplayName("Starting Weight")]
        public decimal StartingWeight { get; set; }

        [DisplayName("Goal Weight")]
        public decimal GoalWeight { get; set; }

        [DisplayName("Current Weight")]
        public decimal CurrentWeight { get; set; }

        [DisplayName("Percent Lost")]
        public decimal PercentLost
        {
            get
            {
                return PoundsLost / StartingWeight * 100;
            }
        }

        [DisplayName("Weight to meet goal")]
        public decimal WeightLeft
        {
            get
            {
                return StartingWeight - PoundsLost - GoalWeight;
            }
        }

        [DisplayName("Pounds Lost")]
        public decimal PoundsLost
        {
            get
            {
                return StartingWeight - CurrentWeight;
            }
        }

        [DisplayName("Weight per week to meet goal")]
        public decimal PerWeek
        {
            get
            {
                if (WeeksLeft > 0)
                {
                    return (WeightLeft / WeeksLeft);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}