using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
using HairBookPro.Models;
using Hairbookpro.Models;

namespace Hairbookpro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}