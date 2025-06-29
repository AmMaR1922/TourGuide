using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Auth
{
    public interface IExternalAuthService
    {
        Task<string> HandleGoogleCallbackAsync();
    }
}
