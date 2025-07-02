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
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public BookingStatus BookingStatus { get; set; } = BookingStatus.Pending;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.HasValue && EndDate.HasValue && EndDate.Value < StartDate.Value)
            {
                yield return new ValidationResult("Start date cannot be later than end date.");
            }
            
        }
    }
}
