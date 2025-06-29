using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.ApplicationUser
{
    public class ApplicationUserLoginDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = null!;


        [Length(8,8,ErrorMessage ="Password muust be Atleast 8 Chars")]
        [Required]
        public string Password { get; set; } = null!;
    }
}
