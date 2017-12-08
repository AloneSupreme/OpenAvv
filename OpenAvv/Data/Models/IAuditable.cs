using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAvv.Data.Models
{
    public interface IAuditable
    {
        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
    }
}
