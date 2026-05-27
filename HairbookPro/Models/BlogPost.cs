using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HairBookPro.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required, StringLength(160)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public int BlogCategoryId { get; set; }
        public virtual BlogCategory BlogCategory { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
