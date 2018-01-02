using OpenAvv.Data.Models.LikeSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAvv.Data.Models.CommentSystem
{
    public class Comment
    {
        public string Id { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public string StoryId { get; set; }
        public string UserId{ get; set; }
        [DefaultValue(0)]
        public int LikeCount { get; set; }
        [DefaultValue(0)]
        public int DislikeCount { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public Story Story { get; set; }
        public OpenAvvUser User { get; set; }
        public ICollection<Reply> Replies { get; set; }
        public ICollection<CommentLike> CommentLikes { get; set; }
    }
}
