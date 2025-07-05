using DomainLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class IncludesTranslation:BaseEntity
    {
        public int IncludeId { get; set; }
        public Includes Includes { get; set; } = null!;

        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;
        public string Name { get; set; } = null!;


    }
}
