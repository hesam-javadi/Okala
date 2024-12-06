using System.Net;
using Newtonsoft.Json;
using Okala.Domain.Exceptions;
using Okala.Domain.Response;

namespace Okala.API.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (StatusCodeException ex)
            {
                ex.ErrorResponse.ErrorMessages.ForEach(em => em.ErrorKey = em.ErrorKey);
                context.Response.StatusCode = (int)ex.StatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(ex.ErrorResponse));
            }
            catch (Exception ex)
            {
                var errorCode = Guid.NewGuid().ToString().Replace("-", "");
                logger.LogError(ex, errorCode);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse(
                    new List<ErrorResponseDetail>
                    {
                        new()
                        {
                            ErrorMessage = "Something went wrong! Please try again later.",
                            ErrorId = errorCode,
                            IsInternalError = true
                        }
                    })));
            }
        }
    }
}