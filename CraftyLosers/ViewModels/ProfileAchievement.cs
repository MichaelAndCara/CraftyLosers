using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CraftyLosers.Models;

namespace CraftyLosers.ViewModels
{
    public class ProfileAchievement
    {
        public bool Unlocked { get; set; }
        public Achievement Achievement { get; set; }
        public int MyQty { get; set; }
    }

    public class ProfileAchievementHeader
    {
        public User User { get; set; }
        public ICollection<ProfileAchievement> ProfileAchievments { get; set; }
    }
}