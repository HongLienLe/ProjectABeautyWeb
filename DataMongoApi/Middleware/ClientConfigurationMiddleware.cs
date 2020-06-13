using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace DataMongoApi.Middleware
{
    public class ClientConfigurationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<string> DatabaseNames;

        public ClientConfigurationMiddleware(RequestDelegate next)
        {
            _next = next;
            DatabaseNames = new List<string>()
            {
                "000000",
                "DefaultDb"
            };
        }

        public async Task InvokeAsync(HttpContext httpContext, IClientConfiguration clientConfiguration)
        {

                httpContext.Request.Headers.TryGetValue("merchant-id", out StringValues merchantId);

            if (DatabaseNames.Contains(merchantId))
            {
                clientConfiguration.MerchantId = merchantId.SingleOrDefault();
            }
            else
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Invalid MerchartdId");
                return;

            }
                await _next.Invoke(httpContext);
        }
    }

    public static class ClientConfigurationExtension
    {
        public static IApplicationBuilder UseClientConfiguration(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<ClientConfigurationMiddleware>();
        }
    }
}
