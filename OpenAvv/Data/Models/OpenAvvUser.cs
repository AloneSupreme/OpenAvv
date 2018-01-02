using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using OpenAvv.Data.Models.ImageSystem;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAvv.Data.Models
{
    public class OpenAvvUser : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
        //[Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public Image Avatar { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public ICollection<Story> Stories { get; set; }
    }

    public class OpenAvvRole : IdentityRole
    {

    }
}
