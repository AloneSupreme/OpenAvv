using OpenAvv.Data.Models;
using OpenAvv.Data.Models.CommentSystem;
using OpenAvv.Data.Models.LikeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAvv.ViewModels
{
    public class CommentViewModel
    {
        public CommentViewModel() { }
        public CommentViewModel(Comment comment)
        {
            Comment = comment;
        }
        public Comment Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public IList<CommentViewModel> ChildReplies { get; set; }
        public string Body { get; set; }
        public string Id { get; set; }
        public string StoryId { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        //public string ParentReplyId { get; set; }
        public string CommenterId { get; set; }
        public string CommenterName { get; set; }
        public string LikeOrDislike { get; set; } //By loggedIn User
        public Story Story { get; set; }
        public OpenAvvUser User { get; set; }
        public ICollection<CommentViewModel> Replies { get; set; }
        public int ReplyCount { get; set; }
        public ICollection<CommentLike> CommentLikes { get; set; }
        public ICollection<ReplyLike> ReplyLikes { get; set; }
    }
}
