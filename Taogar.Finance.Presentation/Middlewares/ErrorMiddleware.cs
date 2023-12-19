using System.Net;
using Taogar.HTTP.Domain.Models.Errors;

namespace Taogar.Finance.Presentation.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, /*IUserService userService,*/ Logger.ILogger logger)
        {
            try
            {
                logger.Info<ErrorMiddleware>("начало запроса", ConsoleColor.Red);
                //userService.SetUser(httpContext.User);
                await _next(context);
                logger.Info<ErrorMiddleware>("конец запроса\n\n\n", ConsoleColor.Red);
            }
            catch (BaseHTTPError ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsync(ex.ToString());
                logger.Info<ErrorMiddleware>($"Пользователь получил предупреждение {ex.ToLog()}\n\n\n", ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                logger.Info<ErrorMiddleware>($"Произошла критическая ошибка {ex.Message}", ConsoleColor.Red);
                await HandleExceptionAsync(context, /*userService*/ logger);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context,/* IUserService userService,*/ Logger.ILogger logger)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var error = new BaseHTTPError(context.Response.StatusCode, /*userService.RequestId,*/
                $"Непредвиденная ошибка, сообщите об этом издателю с указанием TraceId");
            logger.Info<ErrorMiddleware>($"Пользователь получил ошибку {error.ToLog()}\n\n\n", ConsoleColor.Red);
            await context.Response.WriteAsync(error.ToString());
        }


    }
}
