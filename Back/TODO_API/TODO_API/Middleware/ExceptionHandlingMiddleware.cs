using Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace TODO_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var statusCode = ex switch
                {
                    NotFoundException => HttpStatusCode.NotFound,
                    ConflictException => HttpStatusCode.Conflict,
                    UnauthorizedException => HttpStatusCode.Unauthorized,
                    BadRequestException => HttpStatusCode.BadRequest,
                    _ => HttpStatusCode.InternalServerError
                };

                if (statusCode == HttpStatusCode.InternalServerError)
                    _logger.LogError(ex, "Request failed");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var response = new { message = statusCode == HttpStatusCode.InternalServerError ? "An unexpected error occurred." : ex.Message };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
