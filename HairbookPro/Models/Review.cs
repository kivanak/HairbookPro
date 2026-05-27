using System;
using System.ComponentModel.DataAnnotations;

namespace HairBookPro.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int StylistId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Stylist Stylist { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
