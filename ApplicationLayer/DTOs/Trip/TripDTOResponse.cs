using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.TripDtos
{
   public class TripDTOResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }
        public double Rating { get; set; }
        public bool IsBestSeller { get; set; }
        public string Category { get; set; } = null!;
        public int Reviews { get; set; }
        public bool? IsAvailable { get; set; }
        public string? MainImageURL { get; set; }
    }
}
