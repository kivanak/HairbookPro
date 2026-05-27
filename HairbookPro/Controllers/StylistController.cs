using Hairbookpro.Models;
using Hairbookpro.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Hairbookpro.Controllers
{
    public class StylistsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var stylists = db.Stylists
                .Where(s => s.IsActive)
                .OrderBy(s => s.FullName)
                .ToList();

            var ratings = new Dictionary<int, decimal>();

            foreach (var stylist in stylists)
            {
                var rating = db.Database.SqlQuery<decimal>(
                    "SELECT dbo.fn_GetAverageStylistRating(@StylistId)",
                    new SqlParameter("@StylistId", stylist.Id)
                ).FirstOrDefault();

                ratings[stylist.Id] = rating;
            }

            ViewBag.Ratings = ratings;

            return View(stylists);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview(int stylistId, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
            {
                TempData["ReviewError"] = "Ocjena mora biti od 1 do 5.";
                return RedirectToAction("Index");
            }

            var review = new Review
            {
                StylistId = stylistId,
                UserId = User.Identity.GetUserId(),
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            db.Reviews.Add(review);
            db.SaveChanges();

            TempData["ReviewSuccess"] = "Ocjena je uspješno dodata.";
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var stylist = db.Stylists
                .Include(s => s.Reviews)
                .FirstOrDefault(s => s.Id == id);

            if (stylist == null)
            {
                return HttpNotFound();
            }

            return View(stylist);
        }
    }
}