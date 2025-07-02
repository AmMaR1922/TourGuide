using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Booking
{
    public class BookingToBeUpdatedDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Adults must be at least 1.")]
        public int Adults { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Children cannot be negative.")]
        public int Children { get; set; }
        public DateTime TripDate { get; set; }
        public BookingStatus Status { get; set; }
    }
}
