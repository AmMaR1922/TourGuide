using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
   public class Booking
    {

        public int Id { get; set; } 
    
        public DateTime Date { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
    
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public BookingStatus Status { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }


        
    }
}
