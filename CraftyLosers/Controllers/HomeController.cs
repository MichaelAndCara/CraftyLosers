using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CraftyLosers.Repositories;
using CraftyLosers.ViewModels;

namespace CraftyLosers.Controllers
{
    public class HomeController : Controller
    {
        CraftyContext db = new CraftyContext();

        public ActionResult Index()
        {
            var users = db.Users.Include("WeightCheckIns").Where(e =>
                e.WeightCheckIns.Count > 0 &&
                e.StartWeight.Value >= 80 &&
                e.GoalWeight.Value >= 80);

            List<Stats> stats = new List<Stats>();

            foreach (var user in users)
            {
                stats.Add(new Stats(user));
            }

            stats = stats.OrderByDescending(e => e.PercentLost).ToList();

            ViewBag.TotalLost = Convert.ToInt32(stats.Sum(e => e.PoundsLost));

            return View(stats);
        }
    }
}
