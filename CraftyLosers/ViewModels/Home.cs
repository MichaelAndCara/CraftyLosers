using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CraftyLosers.Models;

namespace CraftyLosers.ViewModels
{
    public class Home
    {
        public ICollection<Stats> LoserRank { get; set; }
    }
}