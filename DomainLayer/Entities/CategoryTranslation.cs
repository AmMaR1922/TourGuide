using DomainLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class CategoryTranslation:BaseEntity
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;

        public string Name { get; set; } = null!;




    }
}
