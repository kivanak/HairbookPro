using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Hairbookpro.Models;

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
            var query = db.Services.Include(s => s.ServiceCategory).Where(s => s.IsActive);

            if (!string.IsNullOrWhiteSpace(model.Query))
                query = query.Where(s => s.Name.Contains(model.Query) || s.Description.Contains(model.Query));

            if (model.CategoryId.HasValue)
                query = query.Where(s => s.ServiceCategoryId == model.CategoryId.Value);

            if (model.MinPrice.HasValue)
                query = query.Where(s => s.Price >= model.MinPrice.Value);

            if (model.MaxPrice.HasValue)
                query = query.Where(s => s.Price <= model.MaxPrice.Value);

            if (model.MaxDurationMinutes.HasValue)
                query = query.Where(s => s.DurationMinutes <= model.MaxDurationMinutes.Value);

            model.Results = query.OrderBy(s => s.Price).ToList();
            ViewBag.CategoryId = new SelectList(db.ServiceCategories, "Id", "Name", model.CategoryId);

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
