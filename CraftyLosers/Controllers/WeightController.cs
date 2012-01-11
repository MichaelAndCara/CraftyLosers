﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CraftyLosers.Repositories;
using CraftyLosers.Models;
using CraftyLosers.ViewModels;

namespace CraftyLosers.Controllers
{
    [Authorize]
    public class WeightController : Controller
    {
        CraftyContext db = new CraftyContext();

        public ActionResult WeightCheckIns()
        {
            var user = db.Users.Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

            return View(db.WeightCheckIns.Where(e => e.UserId == user.Id));
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
    }
}