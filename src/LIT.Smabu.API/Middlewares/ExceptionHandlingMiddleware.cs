using LIT.Smabu.Domain.Base;
using System.Net;

namespace LIT.Smabu.API.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Response response = new();
            context.Response.ContentType = "application/json";

            if (exception is DomainException domainException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                response.StatusCode = context.Response.StatusCode;
                response.Message = domainException.Message;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.StatusCode = context.Response.StatusCode;
                response.Message = exception.Message;
            }

            return context.Response.WriteAsync(response.Message);
        }

        internal class Response
        {
            public int StatusCode { get; internal set; }
            public string? Message { get; internal set; }
        }
    }
}