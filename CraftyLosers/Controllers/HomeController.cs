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
                (e.WeightCheckIns.Count > 0 || e.EndWeight != null) &&
                e.StartWeight.Value >= 80 && e.EndWeight != null);
                //(e.GoalWeight.Value >= 80 || e.EndWeight != null));

            List<Stats> stats = new List<Stats>();

            foreach (var user in users)
            {
                var stat = new Stats(user);
                if(stat.PoundsLost >= 0 || stat.OfficialPoundsLost >= 0)
                    stats.Add(new Stats(user));
            }

            ViewBag.TotalLost = Convert.ToInt32(stats.Sum(e => e.PoundsLost));
            ViewBag.OfficialTotalLost = Convert.ToInt32(stats.Sum(e => e.OfficialPoundsLost));

            return View(stats);
        }
    }
}
