using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OpenAvv.Data.Models.CommentSystem;

namespace OpenAvv.Data.Models.LikeSystem
{
    public class CommentLike
    {
        [Key]
        public string Id { get; set; }
        public string CommentId { get; set; }
        public string UserId { get; set; }
        public bool Like { get; set; }
        public bool Dislike { get; set; }

        public Comment Comment { get; set; }
        public OpenAvvUser User { get; set; }
    }
}
