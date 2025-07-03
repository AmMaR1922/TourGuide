using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Booking
{
    public class BookingDTOResponse
    {
        public int Id { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public DateTime TripDate { get; set; }
        public BookingStatus Status { get; set; }
        public decimal TotalCost { get; set; }
        public int TripId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
