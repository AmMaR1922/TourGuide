using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Review
{
    public class ReviewDTORequest
    {
        public string Comment { get; set; } = null!;
        public int Rating { get; set; }
        public int TripId { get; set; }
    }
}
