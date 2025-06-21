using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
   public class TripTranslations
    {
        public int Id { get; set; }
        public string LanguageCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Includes { get; set; }
        public string NotIncluded { get; set; }
        public string Activities { get; set; }

        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
