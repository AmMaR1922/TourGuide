using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class TripReviews : BaseEntity
    {
        public string Comment { get; set; } = null!;
        public int Rating { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

    }
}
