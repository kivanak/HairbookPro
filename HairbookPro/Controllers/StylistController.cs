using Hairbookpro.Models;
using Hairbookpro.Models;
using System.Data.Entity;
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

            return View(stylists);
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