using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.ApplicationUser
{
    public  class ExternalAuthDto
    {
        public string IdToken { get; set; }
        public string Provider { get; set; } = "GOOGLE";
    }
}
