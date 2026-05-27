using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hairbookpro.Models
{
    public class ServiceSearchViewModel
    {
        [Display(Name = "Search")]
        public string Query { get; set; }

        public int? CategoryId { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public int? MaxDurationMinutes { get; set; }

        public IEnumerable<Service> Results { get; set; }
    }
}
