using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using HairBookPro.Models;
using Microsoft.AspNet.Identity;

namespace HairBookPro.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

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

        public ActionResult Create()
        {
            ViewBag.ServiceId = new SelectList(db.Services.Where(s => s.IsActive), "Id", "Name");
            ViewBag.StylistId = new SelectList(db.Stylists.Where(s => s.IsActive), "Id", "FullName");
            return View(new Appointment { AppointmentDate = DateTime.Now.AddDays(1) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment appointment)
        {
            appointment.UserId = User.Identity.GetUserId();
            appointment.Status = "Pending";
            appointment.CreatedAt = DateTime.Now;

            var taken = db.Appointments.Any(a =>
                a.StylistId == appointment.StylistId &&
                a.AppointmentDate == appointment.AppointmentDate &&
                a.Status != "Cancelled");

            if (taken)
                ModelState.AddModelError("", "Izabrani termin je već zauzet.");

            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ServiceId = new SelectList(db.Services.Where(s => s.IsActive), "Id", "Name", appointment.ServiceId);
            ViewBag.StylistId = new SelectList(db.Stylists.Where(s => s.IsActive), "Id", "FullName", appointment.StylistId);
            return View(appointment);
        }
    }
}
