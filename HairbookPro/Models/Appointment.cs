using System;
using System.ComponentModel.DataAnnotations;

namespace HairBookPro.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public int ServiceId { get; set; }
        public int StylistId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required, StringLength(30)]
        public string Status { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Service Service { get; set; }
        public virtual Stylist Stylist { get; set; }
    }
}
