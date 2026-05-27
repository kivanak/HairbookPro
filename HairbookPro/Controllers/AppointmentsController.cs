using Hairbookpro.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Hairbookpro.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Appointments
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var appointments = db.Appointments
                .Include(a => a.Service)
                .Include(a => a.Stylist)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            return View(appointments);
        }

        // GET: Appointments/Create
        public ActionResult Create(int? serviceId)
        {
            ViewBag.ServiceId = new SelectList(
                db.Services.Where(s => s.IsActive),
                "Id",
                "Name",
                serviceId
            );

            ViewBag.StylistId = new SelectList(
                db.Stylists.Where(s => s.IsActive),
                "Id",
                "FullName"
            );

            return View(new Appointment
            {
                AppointmentDate = DateTime.Now.AddDays(1)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment appointment)
        {
            appointment.UserId = User.Identity.GetUserId();
            appointment.Status = "Pending";
            appointment.CreatedAt = DateTime.Now;

            ModelState.Remove("UserId");
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                try
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC dbo.sp_CreateAppointment @UserId, @ServiceId, @StylistId, @AppointmentDate, @Note",
                        new SqlParameter("@UserId", appointment.UserId),
                        new SqlParameter("@ServiceId", appointment.ServiceId),
                        new SqlParameter("@StylistId", appointment.StylistId),
                        new SqlParameter("@AppointmentDate", appointment.AppointmentDate),
                        new SqlParameter("@Note", (object)appointment.Note ?? DBNull.Value)
                    );

                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.ServiceId = new SelectList(
                db.Services.Where(s => s.IsActive),
                "Id",
                "Name",
                appointment.ServiceId
            );

            ViewBag.StylistId = new SelectList(
                db.Stylists.Where(s => s.IsActive),
                "Id",
                "FullName",
                appointment.StylistId
            );

            return View(appointment);
        }
    }
}