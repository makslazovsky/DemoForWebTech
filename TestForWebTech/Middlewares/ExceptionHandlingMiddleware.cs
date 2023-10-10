using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net;
using Serilog;
using System.Text.Json;
using TestForWebTech.Models;
using TestForWebTechBL.Models;

namespace TestForWebTech.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, Serilog.ILogger logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                _logger.Information($"Request to {context.Request.GetDisplayUrl}");
                await _next(context);
                _logger.Information($"Response {context.Response.StatusCode}");
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                ErrorView errorResponse = new ErrorView();
                if (error is BaseException baseError)
                {
                    switch (baseError.ErrorCodes)
                    {
                        case ErrorCodes.NotFound:
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                            break;
                        case ErrorCodes.BadUserInput:
                        case ErrorCodes.AlreadyExists:
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            break;
                        default:
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                    }
                }
                else
                {
                    errorResponse.ErrorCode = ErrorCodes.Unknown;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
               
                await context.Response.WriteAsJsonAsync(errorResponse).ConfigureAwait(false);
            }
        }
    }
}
