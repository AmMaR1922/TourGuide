using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Wishlist : BaseEntity
    {
        public int TripId { get; set; } 
        public Trip Trip { get; set; } = null!;
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
