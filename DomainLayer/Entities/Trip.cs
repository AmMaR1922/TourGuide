using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Common;

namespace DomainLayer.Entities
{
    public class Trip : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime DateTime { get; set; }
        public bool IsBestSeller { get; set; } = false;
        public MeetingPoint MeetingPoint { get; set; } = new MeetingPoint();
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public List<TripActivities> Activities { get; set; } = new List<TripActivities>();
        public List<TripLanguages> TripLanguages { get; set; } = new List<TripLanguages>();
        public List<TripIncludes> TripIncludes { get; set; } = new List<TripIncludes>();
        public List<TripImages> TripImages { get; set; } = new List<TripImages>();
        public List<TripReviews> TripReviews { get; set; } = new List<TripReviews>();
        public List<Booking> TripBookings { get; set; } = new List<Booking>();
        public List<Wishlist>  Wishlist { get; set; } = new List<Wishlist>();
        public ICollection<TripTranslation> TripTranslations { get; set; } = new HashSet<TripTranslation>();    
    }
}
