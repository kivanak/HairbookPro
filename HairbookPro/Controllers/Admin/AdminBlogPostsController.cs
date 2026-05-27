using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Hairbookpro.Models;

namespace Hairbookpro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminBlogPosts
        public ActionResult Index()
        {
            var posts = db.BlogPosts
                .Include(p => p.BlogCategory)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return View(posts);
        }

        // GET: AdminBlogPosts/Create
        public ActionResult Create()
        {
            ViewBag.BlogCategoryId = new SelectList(
                db.BlogCategories,
                "Id",
                "Name"
            );

            return View(new BlogPost());
        }

        // POST: AdminBlogPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogPost post)
        {
            post.CreatedAt = DateTime.Now;

            ModelState.Remove("BlogCategory");
            ModelState.Remove("Comments");

            if (ModelState.IsValid)
            {
                db.BlogPosts.Add(post);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.BlogCategoryId = new SelectList(
                db.BlogCategories,
                "Id",
                "Name",
                post.BlogCategoryId
            );

            return View("~/Views/AdminBlogPosts/Create.cshtml", post);
        }

        // GET: AdminBlogPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = db.BlogPosts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }

            ViewBag.BlogCategoryId = new SelectList(
                db.BlogCategories,
                "Id",
                "Name",
                post.BlogCategoryId
            );

            return View("~/Views/AdminBlogPosts/Edit.cshtml", post);
        }

        // POST: AdminBlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogPost post)
        {
            ModelState.Remove("BlogCategory");
            ModelState.Remove("Comments");

            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.BlogCategoryId = new SelectList(
                db.BlogCategories,
                "Id",
                "Name",
                post.BlogCategoryId
            );

            return View("~/Views/AdminBlogPosts/Edit.cshtml", post);
        }

        // POST: AdminBlogPosts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var post = db.BlogPosts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }

            db.BlogPosts.Remove(post);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}