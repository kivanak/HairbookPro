using System;

namespace Hairbookpro.Models
{
    public class AppointmentLog
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string Action { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
