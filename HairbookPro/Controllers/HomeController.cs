using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Hairbookpro.Models;

namespace Hairbookpro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // HOME PAGE
        public ActionResult Index()
        {
            var model = new HomeViewModel
            {
                Services = db.Services
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Name)
                    .Take(4)
                    .ToList(),

                Stylists = db.Stylists
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.FullName)
                    .Take(3)
                    .ToList(),

                BlogPosts = db.BlogPosts
                    .Include(b => b.BlogCategory)
                    .OrderByDescending(b => b.CreatedAt)
                    .Take(2)
                    .ToList()
            };

            ViewBag.ServicesDropdown = db.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToList();

            return View(model);
        }

        // CONTACT PAGE
        public ActionResult Contact()
        {
            return View(new ContactMessage());
        }

        // CONTACT FORM SUBMIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(FormCollection form)
        {
            var message = new ContactMessage
            {
                FullName = form["FullName"],
                Email = form["Email"],
                Message = form["Message"],
                CreatedAt = DateTime.Now
            };

            TryValidateModel(message);

            if (ModelState.IsValid)
            {
                db.ContactMessages.Add(message);
                db.SaveChanges();

                TempData["Success"] = "Vaša poruka je uspješno poslata.";

                return RedirectToAction("Contact");
            }

            return View(message);
        }

        // ABOUT PAGE
        public ActionResult About()
        {
            ViewBag.Message = "HairBookPro aplikacija za upravljanje frizerskim salonom.";

            return View();
        }
    }
}