using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Booking
{
    public class BookingToBeUpdatedDTO
    {
        public int Adults { get; set; }
        public int Children { get; set; }
        public DateTime TripDate { get; set; }
        public BookingStatus Status { get; set; }
    }
}
