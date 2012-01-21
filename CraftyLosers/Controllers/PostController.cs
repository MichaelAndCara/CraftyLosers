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
        private readonly int ItemsPerPage = 10;

        CraftyContext db = new CraftyContext();

        public ActionResult Posts(int? page = 1)
        {
            int startIndex = CraftyLosers.Util.PageCalculator.StartIndex(page, ItemsPerPage);

            var data = db.Posts.Include("User").Include("Comments.User").OrderByDescending(e => e.DateCreated).ToList();

            int total = data.Count();
            int totalLeft = total - startIndex;

            if (totalLeft < ItemsPerPage)
                return View(new CraftyLosers.Util.PagedList<Post>(data.GetRange(startIndex, totalLeft), page.Value, ItemsPerPage, total));
            else
                return View(new CraftyLosers.Util.PagedList<Post>(data.GetRange(startIndex, ItemsPerPage), page.Value, ItemsPerPage, total));
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
            var comments = db.Comments.Where(e => e.PostId == post.Id);
            foreach (var comment in comments)
            {
                db.Entry(comment).State = System.Data.EntityState.Deleted;
            }
            db.Entry(post).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Posts");
        }

        public ActionResult Comment(int id)
        {
            var post = db.Posts.Find(id);
            return View(
                new Comment() 
                { 
                    Id = 0,
                    UserId = db.Users.Where(e => e.UserName == HttpContext.User.Identity.Name).FirstOrDefault().Id,
                    PostId = id, 
                    Post = post 
                });
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

        public ActionResult EditComment(int id)
        {
            return View(db.Comments.Find(id));
        }

        [HttpPost]
        public ActionResult EditComment(Comment comment)
        {
            db.Entry(comment).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Posts");
        }

        public ActionResult DeleteComment(int id)
        {
            return View(db.Comments.Find(id));
        }
        
        [HttpPost]
        public ActionResult DeleteComment(Comment comment)
        {
            db.Entry(comment).State = System.Data.EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("Posts");
        }
    }
}
