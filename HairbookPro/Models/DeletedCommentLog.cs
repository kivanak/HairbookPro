using System;

namespace HairBookPro.Models
{
    public class DeletedCommentLog
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int BlogPostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
