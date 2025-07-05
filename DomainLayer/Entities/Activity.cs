using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Common;

namespace DomainLayer.Entities
{
    public class Activity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<TripActivities> TripActivities { get; set; } = new List<TripActivities>();
        public ICollection<ActivityTranslation> ActivityTranslations { get; set; }=new HashSet<ActivityTranslation>();
    }
}
