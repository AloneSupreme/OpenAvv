using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAvv.Data.Models.LikeSystem
{
    public class PostLike
    {
        [Key]
        public string Id { get; set; }
        public string StoryId { get; set; }
        public string UserId { get; set; }
        public bool Like { get; set; }
        public bool Dislike { get; set; }

        public Story Story { get; set; }
        public OpenAvvUser User { get; set; }
    }
}
