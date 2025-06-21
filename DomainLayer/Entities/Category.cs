using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
