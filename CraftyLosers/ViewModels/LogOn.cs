using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CraftyLosers.Models;
using System.ComponentModel;

namespace CraftyLosers.ViewModels
{
    public class LogOn
    {
        public User User { get; set; }

        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }
    }
}