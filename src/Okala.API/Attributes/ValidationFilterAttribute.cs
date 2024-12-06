using System.Net;
using Okala.Domain.Exceptions;
using Okala.Domain.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Okala.API.Attributes
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Keys
                    .SelectMany(key => context.ModelState[key].Errors.Select(error => new { Key = key, Error = error.ErrorMessage }))
                    .ToList();
                var errorResponseDetails = errors.Select(e => new ErrorResponseDetail
                {
                    ErrorId = null,
                    ErrorMessage = e.Error,
                    ErrorKey = e.Key.TrimStart('$').TrimStart('.'),
                    IsInternalError = false
                }).ToList();
                throw new StatusCodeException(errorResponseDetails, HttpStatusCode.BadRequest);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
