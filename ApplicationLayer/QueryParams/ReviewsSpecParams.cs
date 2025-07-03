using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.QueryParams
{
    public class ReviewsSpecParams : SpecParams
    {
        public List<string> Sort { get; set; } = new List<string>();
        public int TripId { get; set; }
    }
}
