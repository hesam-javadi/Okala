using Okala.Domain.Response;

namespace Okala.Domain.Exceptions
{
    public class BaseException(List<ErrorResponseDetail> errorResponseDetails)
        : System.Exception(string.Join("\n", errorResponseDetails.Select(e => e.ErrorMessage).ToList()))
    {
        public ErrorResponse ErrorResponse { get; set; } = new(errorResponseDetails);
    }
}
