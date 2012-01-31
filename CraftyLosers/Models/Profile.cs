using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace CraftyLosers.Models
{
    public class Profile
    {
        public Profile(User user, decimal calories)
        {
            User = user;
            Points = Convert.ToInt32(calories);

            LevelPoints = Points;
            Level = 1;
            LevelMax = 100;
            LevelFactor = 1.0M;

            while (LevelMax < Points)
            {
                LevelPoints -= LevelCap;
                Level++;
                LevelFactor += .5M;
                LevelMax = Convert.ToInt32(LevelFactor * LevelCap);
            }

            Achievements = new List<Achievement>();
        }

        public User User { get; set; }

        public int Level { get; set; }

        [DisplayName("Points Earned")]
        public int Points { get; set; }

        [DisplayName("Achievements Unlocked")]
        public ICollection<Achievement> Achievements { get; set; }

        [DisplayName("Quests Completed")]
        public int Quests { get; set; }

        public int LevelPoints { get; set; }
        public int LevelCap { get { return Level * 100; } }
        public int LevelMax { get; set; }
        public decimal LevelFactor { get; set; }
    }
}