namespace Note.Model
{
    public  class BaseResponse : BaseResponse<Object>
    {
        public int IsSuccess { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }

    }

    public class BaseResponse<T>
    {
        public T Data { get; set; }

    }
}
