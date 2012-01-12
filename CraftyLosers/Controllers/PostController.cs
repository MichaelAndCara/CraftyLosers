using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CraftyLosers.Repositories;
using CraftyLosers.Models;

namespace CraftyLosers.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        CraftyContext db = new CraftyContext();

        public ActionResult Posts()
        {
            var data = db.Posts.Include("User").Include("Comments.User").OrderByDescending(e => e.DateCreated);
            return View(data);
        }

        public ActionResult Post()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Post(Post post)
        {
            var user = db.Users.Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

            post.UserId = user.Id;
            post.DateCreated = DateTime.Now;
            post.DateModified = DateTime.Now;
            db.Entry(post).State = System.Data.EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Posts");
        }

        public ActionResult EditPost(int id)
        {
            return View(db.Posts.Find(id));
        }

        [HttpPost]
        public ActionResult EditPost(Post post)
        {
            post.DateModified = DateTime.Now;

            db.Entry(post).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Posts");
        }

        public ActionResult DeletePost(int id)
        {
            return View(db.Posts.Find(id));
        }

        [HttpPost]
        public ActionResult DeletePost(Post post)
        {
            db.Entry(post).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Posts");
        }

        public ActionResult Comment(int id)
        {
            var post = db.Posts.Find(id);
            ViewBag.PostTitle = post.Title;
            ViewBag.PostContent = post.PostContent;
            return View();
        }

        [HttpPost]
        public ActionResult Comment(Comment comment)
        {
            var user = db.Users.Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            comment.CommentDateTime = DateTime.Now;
            comment.UserId = user.Id;

            db.Entry(comment).State = System.Data.EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Posts");
        }
    }
}
