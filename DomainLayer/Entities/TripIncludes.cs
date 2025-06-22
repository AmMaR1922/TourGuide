using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class TripIncludes 
    {
        public bool IsIncluded { get; set; } = true;
        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;
        public int IncludesId { get; set; }
        public Includes Includes { get; set; } = null!;

    }
}
