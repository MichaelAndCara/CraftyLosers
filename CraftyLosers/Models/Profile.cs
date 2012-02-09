using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using CraftyLosers.Repositories;

namespace CraftyLosers.Models
{
    public class Profile
    {
        CraftyContext db = new CraftyContext();

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

            foreach (var achievement in db.Achievements)
            {
                var likeLogs = user.WorkoutLogs.Where(e => e.WorkoutRefId == achievement.WorkoutRefId);

                if (likeLogs.Sum(e => e.Qty) >= achievement.Qty)
                {
                    Achievements.Add(achievement);
                }
            }
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
        public int LevelPercent
        {
            get
            {
                decimal x = (100 / Convert.ToDecimal(LevelCap));

                int y = Convert.ToInt32(LevelPoints * x);

                return y;
            }
        }
    }
}