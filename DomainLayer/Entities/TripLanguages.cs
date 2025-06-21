using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class TripLanguages 
    {
        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;
        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;


    }
}
