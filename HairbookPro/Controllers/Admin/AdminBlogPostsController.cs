using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Hairbookpro.Models;

namespace Hairbookpro.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.BlogPosts.Include(p => p.BlogCategory).OrderByDescending(p => p.CreatedAt).ToList());
        }

        public ActionResult Create()
        {
            ViewBag.BlogCategoryId = new SelectList(db.BlogCategories, "Id", "Name");
            return View(new BlogPost());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogPost post)
        {
            if (ModelState.IsValid)
            {
                post.CreatedAt = DateTime.Now;
                db.BlogPosts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogCategoryId = new SelectList(db.BlogCategories, "Id", "Name", post.BlogCategoryId);
            return View(post);
        }
    }
}
