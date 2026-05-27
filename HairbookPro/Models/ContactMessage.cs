using System;
using System.ComponentModel.DataAnnotations;

namespace HairBookPro.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }

        [Required, StringLength(1200)]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
