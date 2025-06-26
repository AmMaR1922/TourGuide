using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApplicationLayer.DTOs.ApplicationUser
{
    public class ApplicationUserResponseDTO
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string AccessToken { get; set; } = null!;

        public DateTime RefreshTokenExperationDate { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; } = null!;


    }
}
