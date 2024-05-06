using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Shared.Core.Extensions;
using Shared.Core.Utilies.Results;
using System.Net;

namespace Shared.Core.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (ex is ValidationException || ex is FluentValidation.ValidationException validationException)
            {
                var apiResult = new ResultModel
                {
                    StatusCode = HttpStatusCode.BadRequest.ToIntEx(),
                    Message = ex.Message,
                    Errors = ex.ToErrorListEx()
                };
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(apiResult));
            }
            else
            {
                var result = new ResultModel
                {
                    IsSuccess = false,
                    StatusCode = httpContext.Response.StatusCode,
                    Message = $"{ex.Message}",
                };
                await httpContext.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(result));
            }
        }
    }
}