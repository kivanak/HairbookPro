using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Hairbookpro.Models;

namespace Hairbookpro.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminStylistsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Stylists.OrderBy(s => s.FullName).ToList());
        }

        public ActionResult Create()
        {
            return View(new Stylist { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Stylist stylist)
        {
            if (ModelState.IsValid)
            {
                db.Stylists.Add(stylist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stylist);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var stylist = db.Stylists.Find(id);
            if (stylist == null) return HttpNotFound();
            return View(stylist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Stylist stylist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stylist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stylist);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var stylist = db.Stylists.Find(id);
            if (stylist == null) return HttpNotFound();
            return View(stylist);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var stylist = db.Stylists.Find(id);
            db.Stylists.Remove(stylist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
