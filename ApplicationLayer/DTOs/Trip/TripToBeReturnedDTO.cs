using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Trip
{
    public class TripToBeReturnedDTO
    {
        public string Name { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public bool? IsAvailable { get; set; } = true;
        public DateTime DateTime { get; set; }
        public double Rating { get; set; } = 0.0;
        public bool IsBestSeller { get; set; } = false;
        public MeetingPoint MeetingPoint { get; set; } = null!;
        public int CategoryId { get; set; }
        public string MainImage { get; set; } = null!;
        public List<string> Activities { get; set; } = new List<string>();
        public List<string> Languages { get; set; } = new List<string>();
        public List<string> Includes { get; set; } = new List<string>();
        public List<string> NotIncludes { get; set; } = new List<string>();
        public List<string> Images { get; set; } = new List<string>();
    }
}
