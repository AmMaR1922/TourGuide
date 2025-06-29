using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.ApplicationUser
{
    public class ApplicationUserRegisterDTO
    {
        [EmailAddress(ErrorMessage = "Email Format Not valid")]
        [Required]
        public string Email { get; set; } = null!;


        [Required]
        [Length(8,8,ErrorMessage ="Password Must Be at Least 8 chars")]
        public string Password { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;





        

    }
}
