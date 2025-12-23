namespace Note.Domain
{
    public  class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }

    }

    public class BaseResponse<T> :BaseResponse
    {
        public T Data { get; set; }

    }
}
