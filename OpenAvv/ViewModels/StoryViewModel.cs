using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OpenAvv.Data.Models;
using OpenAvv.Data.Models.CommentSystem;
using OpenAvv.Data.Models.LikeSystem;

namespace OpenAvv.ViewModels
{
    public class StoryViewModel
    {


        public string Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "The field Title must be with a minimum 3 characters.")]
        public string Title { get; set; }
        [Display(Name = "Author")]
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        public string AuthorDescription { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "The field Title must be with a Maximum of 100 characters.")]
        public string Description { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public int NetLikeCount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
        public ICollection<CommentViewModel> Replies { get; set; }
        public ICollection<PostLike> PostLikes { get; set; }
    }
}
