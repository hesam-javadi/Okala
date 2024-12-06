namespace Okala.Domain.Response
{
    public class ErrorResponse : BaseResponse
    {
        public ErrorResponse(List<ErrorResponseDetail> errorMessages)
        {
            ErrorMessages = errorMessages;
            IsSuccess = false;
        }

        public List<ErrorResponseDetail> ErrorMessages { get; set; }
    }

    public class ErrorResponseDetail
    {
        public string ErrorMessage { get; set; } = null!;
        public string? ErrorKey { get; set; }
        public string? ErrorId { get; set; }
        public bool IsInternalError { get; set; }
    }
}
