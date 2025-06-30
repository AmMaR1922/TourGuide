using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Auth
{
    public interface ITokenServices
    {
        Task<string> GetAccessToken(ApplicationUser User);
        RefreshToken GetRefreshToken();
      

    }
}
