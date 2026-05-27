using System.ComponentModel.DataAnnotations;

namespace Hairbookpro.Models
{
    public class GalleryImage
    {
        public int Id { get; set; }

        [Required, StringLength(250)]
        public string ImageUrl { get; set; }

        [StringLength(150)]
        public string Title { get; set; }
    }
}
