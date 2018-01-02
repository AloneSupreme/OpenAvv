using OpenAvv.Data.Models.CommentSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAvv.Data.Models.LikeSystem
{
    public class ReplyLike
    {
        [Key]
        public string Id { get; set; }
        public string ReplyId { get; set; }
        public string UserId { get; set; }
        public bool Like { get; set; }
        public bool Dislike { get; set; }

        public Reply Reply { get; set; }
        public OpenAvvUser User { get; set; }
    }
}
