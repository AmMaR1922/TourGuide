using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.QueryParams
{
    public class TripSpecParams : SpecParams
    {
        public List<string>? Sort { get; set; } = new List<string>() {"random"};
        public int? CategoryId { get; set; }
        public int? LanguageId { get; set; }
        public bool? IsBestSeller { get; set; }
        public bool? IsTopRated { get; set; }
        public bool? IsAvailable { get; set; } = true;
    }
}
