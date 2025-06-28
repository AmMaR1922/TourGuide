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
        public double Rating { get; set; } = 0.0;
        public bool IsBestSeller { get; set; } = false;
        public MeetingPoint MeetingPoint { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<TripActivities> Activities { get; set; } = new List<TripActivities>();
        public ICollection<TripLanguages> TripLanguages { get; set; } = new List<TripLanguages>();
        public ICollection<TripIncludes> TripIncludes { get; set; } = new List<TripIncludes>();
        public ICollection<TripImages> TripImages { get; set; } = new List<TripImages>();
        public ICollection<TripReviews> TripReviews { get; set; } = new List<TripReviews>();
        public ICollection<Booking> TripBookings { get; set; } = new List<Booking>();
        public ICollection<Wishlist>  Wishlist { get; set; } = new List<Wishlist>();
    }
}
