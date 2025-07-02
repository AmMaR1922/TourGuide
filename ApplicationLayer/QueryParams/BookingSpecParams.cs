using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.QueryParams
{
    public class BookingSpecParams : SpecParams, IValidatableObject
    {
        public string? Sort {  get; set; }
        public int? TripId { get; set; }
        public int? UserId { get; set; }
        public DateTime? TripStartDate { get; set; }
        public DateTime? TripEndDate { get; set; }
        public DateTime? CreatedStartDate { get; set; }
        public DateTime? CreatedEndDate { get; set; }
        public BookingStatus BookingStatus { get; set; } = BookingStatus.Pending;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TripStartDate.HasValue && TripEndDate.HasValue && TripEndDate.Value < TripStartDate.Value)
            {
                yield return new ValidationResult("Start date cannot be later than end date.");
            }
            if (CreatedStartDate.HasValue && CreatedEndDate.HasValue && CreatedEndDate.Value < CreatedStartDate.Value)
            {
                yield return new ValidationResult("Created start date cannot be later than created end date.");
            }
            
        }
    }
}
