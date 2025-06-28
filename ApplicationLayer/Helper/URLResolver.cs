using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Helper
{
    public static class URLResolver
    {
        private static IHttpContextAccessor? _accessor;

        public static void Init(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public static string? BuildFileUrl(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath) || _accessor?.HttpContext?.Request == null)
                return null;

            var request = _accessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}/{relativePath}";
        }

    }
}
