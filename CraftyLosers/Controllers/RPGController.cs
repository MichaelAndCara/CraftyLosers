﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CraftyLosers.Repositories;
using CraftyLosers.Models;
using CraftyLosers.ViewModels;
using System.Data;

namespace CraftyLosers.Controllers
{
    [Authorize]
    public class RPGController : Controller
    {
        CraftyContext db = new CraftyContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            var user = db.Users.Include("WorkoutLogs").Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

            decimal points = user.WorkoutLogs.Sum(e => e.Calories);

            var profile = new Profile(user, points);

            ViewBag.PGText = "Level " + profile.Level.ToString() + " - " + profile.LevelPoints.ToString() + "/" + (profile.Level * 100).ToString();

            return View(profile);
        }

        public JsonResult GetLevel()
        {
            var user = db.Users.Include("WorkoutLogs").Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

            decimal points = user.WorkoutLogs.Sum(e => e.Calories);

            var profile = new Profile(user, points);

            decimal x = (100 / Convert.ToDecimal(profile.LevelCap));

            int y = Convert.ToInt32(profile.LevelPoints * x);

            return Json(y, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WorkoutLog()
        {
            var user = db.Users.Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

            return View(db.WorkoutLogs.Include("WorkoutRef").Where(e => e.UserId == user.Id).OrderByDescending(e => e.WorkoutDate).ThenByDescending(e => e.Id));
        }

        public ActionResult Log()
        {
            return View(new ViewModels.Workout());
        }

        [HttpPost]
        public ActionResult Log(Workout workout)
        {
            var user = db.Users.Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

            if (workout.Duration.HasValue && workout.Duration.Value > 0)
            {
                var workoutRef = db.WorkoutRefs.Where(e => e.Id == workout.DurationId).FirstOrDefault();

                var workoutLog = new WorkoutLog();
                workoutLog.Qty = workout.Duration.Value;
                workoutLog.UserId = user.Id;
                workoutLog.WorkoutDate = Convert.ToDateTime(workout.WorkoutDate);
                workoutLog.WorkoutRefId = workoutRef.Id;
                workoutLog.Calories = workoutRef.Calories * workout.Duration.Value;

                db.Entry(workoutLog).State = System.Data.EntityState.Added;
                db.SaveChanges();
            }

            if (workout.Rep.HasValue && workout.Rep.Value > 0)
            {
                var workoutRef = db.WorkoutRefs.Where(e => e.Id == workout.RepId).FirstOrDefault();

                var workoutLog = new WorkoutLog();
                workoutLog.Qty = workout.Rep.Value;
                workoutLog.UserId = user.Id;
                workoutLog.WorkoutDate = Convert.ToDateTime(workout.WorkoutDate);
                workoutLog.WorkoutRefId = workoutRef.Id;
                workoutLog.Calories = workoutRef.Calories * workout.Rep.Value;

                db.Entry(workoutLog).State = System.Data.EntityState.Added;
                db.SaveChanges();
            }

            return RedirectToAction("WorkoutLog");
        }

        public ActionResult EditLog(int id)
        {
            return View(db.WorkoutLogs.Include("WorkoutRef").Where(e => e.Id == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult EditLog(WorkoutLog workoutLog)
        {
            var workoutRef = db.WorkoutRefs.Find(workoutLog.WorkoutRefId);
            workoutLog.Calories = workoutRef.Calories * workoutLog.Qty;

            db.Entry(workoutLog).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("WorkoutLog");
        }

        public ActionResult DeleteLog(int id)
        {
            return View(db.WorkoutLogs.Include("WorkoutRef").Where(e => e.Id == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult DeleteLog(WorkoutLog workoutLog)
        {
            db.Entry(workoutLog).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("WorkoutLog");
        }

        public ActionResult Leaderboard()
        {
            var users = db.Users.Include("WorkoutLogs").Where(e =>
                e.WeightCheckIns.Count > 0 &&
                e.StartWeight.Value >= 80 &&
                e.GoalWeight.Value >= 80);

            var profiles = new List<Profile>();

            foreach (var user in users)
            {
                decimal points = user.WorkoutLogs.Sum(e => e.Calories);
                profiles.Add(new Profile(user, points));
            }

            profiles = profiles.OrderByDescending(e => e.Points).ToList();

            return View(profiles);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}