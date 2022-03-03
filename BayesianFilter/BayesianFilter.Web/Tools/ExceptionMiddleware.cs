using BayesianFilter.Web.Tools.Logger;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BayesianFilter.Web.Tools
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostLogger logger;

        public ExceptionMiddleware(RequestDelegate next, IHostLogger logger)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                logger.Error($"Middleware Handler {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new
            {
                context.Response.StatusCode,
                exception.Message
            }.ToString());
        }
    }
}
