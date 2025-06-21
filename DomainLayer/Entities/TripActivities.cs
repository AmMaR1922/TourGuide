using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class TripActivities : BaseEntity
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;
        public int ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;
    }
}
