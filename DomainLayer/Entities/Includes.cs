using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Common;

namespace DomainLayer.Entities
{
    public class Includes : BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<TripIncludes> TripIncludes { get; set; } = new List<TripIncludes>();
        public ICollection<IncludesTranslation> IncludesTranslations { get; set; } = new HashSet<IncludesTranslation>();
    }
}
