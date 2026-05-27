using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HairBookPro.Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
