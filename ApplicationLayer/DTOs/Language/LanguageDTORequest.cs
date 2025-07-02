using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.Language
{
    public class LanguageDTORequest
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
