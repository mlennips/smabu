using LIT.Smabu.Domain.Base;
using System.Net;

namespace LIT.Smabu.API.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private static readonly Action<ILogger, Exception> LogUnhandledException =
            LoggerMessage.Define(LogLevel.Error, new EventId(0, nameof(ExceptionHandlingMiddleware)), "An unhandled exception has occurred.");

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                LogUnhandledException(logger, ex);
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

        internal sealed class Response
        {
            public int StatusCode { get; internal set; }
            public string? Message { get; internal set; }
        }
    }
}