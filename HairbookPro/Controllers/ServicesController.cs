using Hairbookpro.Models;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Hairbookpro.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(string q)
        {
            var services = db.Services.Include(s => s.ServiceCategory).Where(s => s.IsActive);

            if (!string.IsNullOrWhiteSpace(q))
            {
                services = services.Where(s =>
                    s.Name.Contains(q) ||
                    s.Description.Contains(q) ||
                    s.ServiceCategory.Name.Contains(q));
            }

            ViewBag.Query = q;
            return View(services.OrderBy(s => s.Name).ToList());
        }

        public ActionResult Search()
        {
            ViewBag.CategoryId = new SelectList(db.ServiceCategories, "Id", "Name");
            return View(new ServiceSearchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(ServiceSearchViewModel model)
        {
            var results = db.Database.SqlQuery<Service>(
                "EXEC dbo.sp_SearchServices @Query, @CategoryId, @MinPrice, @MaxPrice, @MaxDurationMinutes",
                new SqlParameter("@Query", (object)model.Query ?? DBNull.Value),
                new SqlParameter("@CategoryId", (object)model.CategoryId ?? DBNull.Value),
                new SqlParameter("@MinPrice", (object)model.MinPrice ?? DBNull.Value),
                new SqlParameter("@MaxPrice", (object)model.MaxPrice ?? DBNull.Value),
                new SqlParameter("@MaxDurationMinutes", (object)model.MaxDurationMinutes ?? DBNull.Value)
            ).ToList();

            model.Results = results;

            ViewBag.CategoryId = new SelectList(
                db.ServiceCategories,
                "Id",
                "Name",
                model.CategoryId
            );

            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var service = db.Services.Include(s => s.ServiceCategory).FirstOrDefault(s => s.Id == id);

            if (service == null) return HttpNotFound();

            return View(service);
        }
    }
}
