using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevWiki.API.Middleware
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
                _logger.LogError(ex, "An unhandled exception has occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";
            
            var statusCode = StatusCodes.Status500InternalServerError;
            var title = "An error occurred while processing your request.";
            var detail = exception.Message;

            // Here we can map specific exceptions to specific HTTP status codes.
            // For example:
            // if (exception is ValidationException validationException)
            // {
            //     statusCode = StatusCodes.Status400BadRequest;
            //     title = "Validation Error";
            //     detail = "One or more validation errors occurred.";
            // }
            // else if (exception is NotFoundException notFoundException)
            // {
            //     statusCode = StatusCodes.Status404NotFound;
            //     title = "Resource not found.";
            // }

            context.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };

            var result = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(result);
        }
    }
}
