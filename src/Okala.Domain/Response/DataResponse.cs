namespace Okala.Domain.Response
{
    public class DataResponse<T> : BaseResponse 
    {
        public DataResponse(T data)
        {
            Data = data;
            IsSuccess = true;
        }

        public T Data { get; set; }
    }
}
