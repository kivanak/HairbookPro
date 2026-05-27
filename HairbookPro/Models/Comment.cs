using System;
using System.ComponentModel.DataAnnotations;

namespace Hairbookpro.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int BlogPostId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required, StringLength(1000)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual BlogPost BlogPost { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
