using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Models
{
    public class MailSettings
    {
        public string Email { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Host { get; set; } = null!;

        public int Port { get; set; }
    }
}
