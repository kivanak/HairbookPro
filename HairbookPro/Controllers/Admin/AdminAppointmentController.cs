using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Hairbookpro.Models;

namespace Hairbookpro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminAppointmentsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var appointments = db.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Include(a => a.Stylist)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            return View(appointments);
        }
    }
}