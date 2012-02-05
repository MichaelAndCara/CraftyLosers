using System;
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
        private readonly int ItemsPerPage = 10;

        CraftyContext db = new CraftyContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            var user = db.Users.Include("WorkoutLogs.WorkoutRef").Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

            decimal points = user.WorkoutLogs.Sum(e => e.Calories);

            var profile = new Profile(user, points);

            ViewBag.PGText = "Level " + profile.Level.ToString() + " - " + profile.LevelPoints.ToString() + "/" + (profile.Level * 100).ToString();

            return View(profile);
        }

        public ActionResult ViewProfile(int id)
        {
            var user = db.Users.Include("WorkoutLogs.WorkoutRef").Where(e => e.Id == id).FirstOrDefault();

            decimal points = user.WorkoutLogs.Sum(e => e.Calories);

            var profile = new Profile(user, points);

            ViewBag.PGText = "Level " + profile.Level.ToString() + " - " + profile.LevelPoints.ToString() + "/" + (profile.Level * 100).ToString();

            return View(profile);
        }

        public JsonResult GetLevel(int id)
        {
            var user = db.Users.Include("WorkoutLogs").Where(e => e.Id == id).FirstOrDefault();

            decimal points = user.WorkoutLogs.Sum(e => e.Calories);

            var profile = new Profile(user, points);

            decimal x = (100 / Convert.ToDecimal(profile.LevelCap));

            int y = Convert.ToInt32(profile.LevelPoints * x);

            return Json(y, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WorkoutLog(int? page = 1)
        {
            int startIndex = CraftyLosers.Util.PageCalculator.StartIndex(page, ItemsPerPage);

            var user = db.Users.Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

            var logs = db.WorkoutLogs.Include("WorkoutRef").Where(e => e.UserId == user.Id).OrderByDescending(e => e.WorkoutDate).ThenByDescending(e => e.Id).ToList();

            int total = logs.Count();
            int totalLeft = total - startIndex;

            if (totalLeft < ItemsPerPage)
                return View(new CraftyLosers.Util.PagedList<WorkoutLog>(logs.GetRange(startIndex, totalLeft), page.Value, ItemsPerPage, total));
            else
                return View(new CraftyLosers.Util.PagedList<WorkoutLog>(logs.GetRange(startIndex, ItemsPerPage), page.Value, ItemsPerPage, total));
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
            var users = db.Users.Include("WorkoutLogs").Where(e => e.StartWeight.Value >= 80  & e.Active == true);

            var profiles = new List<Profile>();

            foreach (var user in users)
            {
                decimal points = user.WorkoutLogs.Sum(e => e.Calories);
                profiles.Add(new Profile(user, points));
            }

            profiles = profiles.OrderByDescending(e => e.Points).ToList();

            return View(profiles);
        }

        //public ActionResult Achievements()
        //{
        //    var user = db.Users.Include("WorkoutLogs").Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

        //    return RedirectToAction("Achievements", new { id = user.Id });
        //    //var profileAchievements = new List<ProfileAchievement>();

        //    //foreach (var achievement in db.Achievements)
        //    //{
        //    //    var likeLogs = user.WorkoutLogs.Where(e => e.WorkoutRefId == achievement.WorkoutRefId);

        //    //    var profileAchievement = new ProfileAchievement();
        //    //    profileAchievement.Achievement = achievement;
        //    //    var soFar = likeLogs.Sum(e => e.Qty);
        //    //    profileAchievement.MyQty = Math.Min(likeLogs.Sum(e => e.Qty), achievement.Qty);

        //    //    if (likeLogs.Sum(e => e.Qty) >= achievement.Qty)
        //    //    {
        //    //        profileAchievement.Unlocked = true;
        //    //    }

        //    //    profileAchievements.Add(profileAchievement);
        //    //}

        //    //return View(profileAchievements);
        //}

        public ActionResult Achievements(int? id)
        {
            var myAchievements = new ProfileAchievementHeader();
            if (id.HasValue)
            {
                myAchievements.User = db.Users.Include("WorkoutLogs").Where(e => e.Id == id).FirstOrDefault();
            }
            else
            {
                myAchievements.User = db.Users.Include("WorkoutLogs").Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            }

            myAchievements.ProfileAchievments = new List<ProfileAchievement>();

            foreach (var achievement in db.Achievements)
            {
                var likeLogs = myAchievements.User.WorkoutLogs.Where(e => e.WorkoutRefId == achievement.WorkoutRefId);

                var profileAchievement = new ProfileAchievement();
                profileAchievement.Achievement = achievement;
                var soFar = likeLogs.Sum(e => e.Qty);
                profileAchievement.MyQty = Math.Min(likeLogs.Sum(e => e.Qty), achievement.Qty);

                if (likeLogs.Sum(e => e.Qty) >= achievement.Qty)
                {
                    profileAchievement.Unlocked = true;
                }

                myAchievements.ProfileAchievments.Add(profileAchievement);
            }

            return View(myAchievements);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
