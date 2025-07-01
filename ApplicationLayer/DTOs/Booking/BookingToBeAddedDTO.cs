using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Booking
{
    public class BookingToBeAddedDTO : IValidatableObject
    {
        [Range(1, int.MaxValue, ErrorMessage = "Adults must be at least 1.")]
        public int Adults { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Children cannot be negative.")]
        public int Children { get; set; }
        public DateTime TripDate { get; set; }
        public int TripId { get; set; }
        public int UserId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TripDate <= DateTime.Now)
            {
                yield return new ValidationResult("Invalid Date");
            }
        }
    }
}
