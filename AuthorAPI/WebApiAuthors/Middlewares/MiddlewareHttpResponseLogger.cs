using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace WebApiAuthors.Middlewares
{
    // Class to extend the ApplicationBuilder constructor and add the custom middleware to app
    public static class MiddlewareHttpResponseLoggerExtensions
    {
        public static IApplicationBuilder UseHttpResponseLogger(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MiddlewareHttpResponseLogger>();
        }
    }

    public class MiddlewareHttpResponseLogger
	{
        private readonly RequestDelegate next;
        private readonly ILogger<MiddlewareHttpResponseLogger> logger;

        public MiddlewareHttpResponseLogger(RequestDelegate next, ILogger<MiddlewareHttpResponseLogger> logger)
		{
            this.next = next;
            this.logger = logger;
        }

        // Inkove or InvokeAsync
        public async Task InvokeAsync(HttpContext context)
        {
            using (var memoryStream = new MemoryStream())
            {
                var originalBodyResponse = context.Response.Body;
                context.Response.Body = memoryStream;

                await next(context);

                memoryStream.Seek(0, SeekOrigin.Begin);
                string response = new StreamReader(memoryStream).ReadToEnd();
                memoryStream.Seek(0, SeekOrigin.Begin);

                await memoryStream.CopyToAsync(originalBodyResponse);
                context.Response.Body = originalBodyResponse;

                logger.LogInformation(response);
            }
        }
    }
}

