using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hairbookpro.Models
{
    public class BlogCategory
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }

        public virtual ICollection<BlogPost> BlogPosts { get; set; }
    }
}
