using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HairBookPro.Models;

namespace HairBookPro.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminServicesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Services.Include(s => s.ServiceCategory).OrderBy(s => s.Name).ToList());
        }

        public ActionResult Create()
        {
            ViewBag.ServiceCategoryId = new SelectList(db.ServiceCategories, "Id", "Name");
            return View(new Service { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                db.Services.Add(service);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ServiceCategoryId = new SelectList(db.ServiceCategories, "Id", "Name", service.ServiceCategoryId);
            return View(service);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var service = db.Services.Find(id);
            if (service == null) return HttpNotFound();

            ViewBag.ServiceCategoryId = new SelectList(db.ServiceCategories, "Id", "Name", service.ServiceCategoryId);
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ServiceCategoryId = new SelectList(db.ServiceCategories, "Id", "Name", service.ServiceCategoryId);
            return View(service);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var service = db.Services.Find(id);
            if (service == null) return HttpNotFound();
            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var service = db.Services.Find(id);
            db.Services.Remove(service);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
