using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CraftyLosers.Repositories;
using CraftyLosers.Models;
using CraftyLosers.ViewModels;
using System.IO;
using System.Web.Helpers;

namespace CraftyLosers.Controllers
{
    [Authorize]
    public class WeightController : Controller
    {
        private readonly int ItemsPerPage = 10;

        CraftyContext db = new CraftyContext();

        public ActionResult WeightCheckIns(int? page = 1)
        {
            int startIndex = CraftyLosers.Util.PageCalculator.StartIndex(page, ItemsPerPage);

            var user = db.Users.Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

            var checkIns = db.WeightCheckIns.Where(e => e.UserId == user.Id).ToList();

            int total = checkIns.Count();
            int totalLeft = total - startIndex;

            if (totalLeft < ItemsPerPage)
                return View(new CraftyLosers.Util.PagedList<WeightCheckIn>(checkIns.GetRange(startIndex, totalLeft), page.Value, ItemsPerPage, total));
            else
                return View(new CraftyLosers.Util.PagedList<WeightCheckIn>(checkIns.GetRange(startIndex, ItemsPerPage), page.Value, ItemsPerPage, total));
        }

        public ActionResult CheckIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckIn(CheckIn CheckIn)
        {
            var user = db.Users.Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

            var weightCheckIn = new WeightCheckIn()
            {
                UserId = user.Id,
                CheckInDate = CheckIn.CheckInDate,
                Weight = CheckIn.Weight
            };

            db.Entry(weightCheckIn).State = System.Data.EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("WeightCheckIns");
        }

        public ActionResult Delete(int id)
        {
            return View(db.WeightCheckIns.Find(id));
        }

        [HttpPost]
        public ActionResult Delete(WeightCheckIn weightCheckIn)
        {
            db.Entry(weightCheckIn).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("WeightCheckIns");
        }

        public ActionResult Edit(int id)
        {
            return View(db.WeightCheckIns.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(WeightCheckIn weightCheckIn)
        {
            db.Entry(weightCheckIn).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("WeightCheckIns");
        }

        public ActionResult Stats()
        {
            var user = db.Users.Include("WeightCheckIns").Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

            if (user.WeightCheckIns.Count > 0 && Convert.ToDecimal(user.GoalWeight) > 80)
            {
                var stats = new Stats(user);

                return View(stats);
            }
            else
            {
                ModelState.AddModelError("", "You need to have your Intial Weight, Goal Weight, and at least one Check In Weight entered to view Stats.");
                return View();
            }
        }

        public ActionResult GetChart()
        {
            var user = db.Users.Include("WeightCheckIns").Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            var weights = user.WeightCheckIns.Select(e => e.Weight).ToList();
            var dates = user.WeightCheckIns.Select(e => e.CheckInDate).ToList();
            var chart = new Chart(width: 600, height: 440, theme: ChartTheme.Blue);
            chart.SetYAxis(min: Convert.ToDouble(Math.Round(weights.Min()) - 1), max: Convert.ToDouble(Math.Round(weights.Max()) + 1));
            chart.AddSeries(
                    name: "Checkins",
                    chartType: "Line",
                    chartArea: null,
                    xValue: dates,
                    xField: "Checkin Dates",
                    yValues: weights,
                    yFields: "Weights").Write();

            return null;
        }
    }
}
