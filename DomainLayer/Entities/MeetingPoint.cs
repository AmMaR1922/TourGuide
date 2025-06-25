using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    [Owned]
    public class MeetingPoint
    {
        public string Address { get; set; } = null!;
        public string URL { get; set; } = null!;
    }
}
