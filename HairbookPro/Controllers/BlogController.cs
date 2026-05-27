using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HairBookPro.Models;
using Microsoft.AspNet.Identity;

namespace HairBookPro.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var posts = db.BlogPosts.Include(p => p.BlogCategory)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return View(posts);
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var post = db.BlogPosts
                .Include(p => p.BlogCategory)
                .Include(p => p.Comments.Select(c => c.User))
                .FirstOrDefault(p => p.Id == id);

            if (post == null) return HttpNotFound();

            return View(post);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(int blogPostId, string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                db.Comments.Add(new Comment
                {
                    BlogPostId = blogPostId,
                    Content = content,
                    UserId = User.Identity.GetUserId(),
                    CreatedAt = DateTime.Now
                });

                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = blogPostId });
        }
    }
}
