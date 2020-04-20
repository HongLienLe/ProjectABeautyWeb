//using System;
//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;

//namespace DataMongoApi.Middleware
//{
//    public class LoggerMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ILogger _logger;

//        public LoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
//        {
//            _next = next;
//            _logger = loggerFactory.CreateLogger<LoggerMiddleware>();
//        }

//        public async Task InvokeAsync(HttpContext httpContext)
//        {
//            //Read body from the request and log it
//            using (var reader = new StreamReader(httpContext.Request.Body))
//            {
//                var requestBody = reader.ReadToEnd();
//                //As this is a middleware below line will make sure it will log each and every request body
//                _logger.LogInformation(requestBody);
//            }

//            //Move to next delegate/middleware in the pipleline
//            await _next.Invoke(httpContext);
//        }
//    }

//    public static class LoggerMiddlewareExtension
//    {
//        public static IApplicationBuilder UseLogger(this IApplicationBuilder applicationBuilder)
//        {
//            return applicationBuilder.UseMiddleware<LoggerMiddleware>();
//        }
//    }
//}
