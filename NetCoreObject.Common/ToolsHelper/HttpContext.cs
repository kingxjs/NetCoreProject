using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreObject.Common
{
    public static class HttpContextHelper
    {
        private static IHttpContextAccessor _accessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;

        public static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }
}
