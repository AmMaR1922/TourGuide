using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Review
{
    public class ReviewDTOResponse
    {
        public string Comment { get; set; } = null!;
        public int Rating { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
