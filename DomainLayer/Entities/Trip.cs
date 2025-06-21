using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
   public class Trip
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Duration { get; set; }
        public string Category { get; set; } 
        public List<string> ImageUrls { get; set; }
        public ICollection<TripTranslations> TripTranslations { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }

    }
}
