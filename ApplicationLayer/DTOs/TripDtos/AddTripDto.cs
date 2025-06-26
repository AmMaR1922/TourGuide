using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.TripDtos
{
  public  class AddTripDto
    {

        public string Name { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DateTime { get; set; }
        public double Rating { get; set; }
        public string MeetingPointAddress { get; set; }
        public string MeetingPointURL { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<string> Activities { get; set; }
        public List<string> Languages { get; set; }
        public List<string> Includes { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<string> Reviews { get; set; }
    }

}
 