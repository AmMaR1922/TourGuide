﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    [Owned]
    public class RefreshToken
    {

        public string Token { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public DateTime ExpireOn { get; set; }
        public bool IsExpire => DateTime.UtcNow > ExpireOn.ToUniversalTime();

        public DateTime? RevokedOn { get; set; }
        public bool IsActive => (!IsExpire && RevokedOn is null);


    }
}
