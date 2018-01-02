using OpenAvv.Data.Models.CommentSystem;
using OpenAvv.Data.Models.LikeSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAvv.Data.Models
{
    public class Story : IAuditable
    {
        public string Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "The field Title must be with a minimum 3 characters.")]
        public string Title { get; set; }
        public string AuthorId { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "The field Title must be with a Maximum of 100 characters.")]
        public string Description { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        [DefaultValue(false)]
        public bool Published { get; set; }

        [DefaultValue(0)]
        public int NetLikeCount { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reply> Replies { get; set; }
        public ICollection<PostLike> PostLikes { get; set; }

        public OpenAvvUser Author { get; set; }
    }
}
