using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<TripReviews> Reviews { get; set; } = new List<TripReviews>();
        public ICollection<Wishlist> Wishlist { get; set; } = new List<Wishlist>();
        public  HashSet<RefreshToken> ?RefreshTokens { get; set; } = new HashSet<RefreshToken>();

    }
}
