using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs.TripDtos
{
    public  class TripToBeAddedDTO
    {
        public string Name { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DateTime { get; set; }
        public string MeetingPointAddress { get; set; } = null!;
        public string MeetingPointURL { get; set; } = null!;
        public int CategoryId { get; set; }
        public IFormFile MainImage { get; set; } = null!;
        public List<int> Activities { get; set; } = new List<int>();
        public List<int> Languages { get; set; } = new List<int>();
        public List<int> Includes { get; set; } = new List<int>();
        public List<int> NotIncludes { get; set; } = new List<int>();
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}
 