using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HairBookPro.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        public bool IsActive { get; set; }

        public int ServiceCategoryId { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
