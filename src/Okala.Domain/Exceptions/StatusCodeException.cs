using System.Net;
using Okala.Domain.Response;

namespace Okala.Domain.Exceptions
{
    public class StatusCodeException(List<ErrorResponseDetail> errorResponseDetails, HttpStatusCode statusCode)
        : BaseException(errorResponseDetails)
    {
        public HttpStatusCode StatusCode { get; set; } = statusCode;
    }
}
