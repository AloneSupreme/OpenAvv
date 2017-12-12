using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAvv.Data.Entities
{
    public class Story
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public Person Author { get; set; }
        public string Info { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
