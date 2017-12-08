using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OpenAvv.Data.Models;

namespace OpenAvv.ViewModels
{
    public class StoryVM
    {


        public int ID { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "The field Title must be with a minimum 3 characters.")]
        public string Title { get; set; }
        [Display(Name = "Author")]
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "The field Title must be with a Maximum of 100 characters.")]
        public string Info { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
