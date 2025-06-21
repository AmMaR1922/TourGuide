using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Booking : BaseEntity
    {
        public int Adults { get; set; }
        public int Children { get; set; }
        public DateTime TripDate { get; set; } 
        public BookingStatus Status { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        //Discount  
    }
}
