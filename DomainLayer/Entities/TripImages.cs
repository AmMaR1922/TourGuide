using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Common;

namespace DomainLayer.Entities
{
    public class TripImages : BaseEntity
    {
        public string ImageURL { get; set; } = null!;
        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;
    }
}
