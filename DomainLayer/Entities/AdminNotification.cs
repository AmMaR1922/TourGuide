using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class AdminNotification
    {

        public int Id { get; set; }
        public string Type { get; set; }
     
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? RelatedBookingId { get; set; }
        public Booking RelatedBooking { get; set; }
    }
}
