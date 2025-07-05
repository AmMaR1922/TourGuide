using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Common;

namespace DomainLayer.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<Trip> Trips { get; set; } = new HashSet<Trip>();
        public ICollection<CategoryTranslation> CategoryTranslations { get; set; }  =new HashSet<CategoryTranslation>(); 
    }
}
