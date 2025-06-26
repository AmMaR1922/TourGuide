using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Common;

namespace DomainLayer.Entities
{
    public class Language : BaseEntity
    {
       public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public ICollection<TripLanguages> TripLanguages { get; set; } = new List<TripLanguages>();
    }
}
