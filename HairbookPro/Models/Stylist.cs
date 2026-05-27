using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HairBookPro.Models
{
    public class Stylist
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string Specialization { get; set; }

        [StringLength(1000)]
        public string Bio { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
