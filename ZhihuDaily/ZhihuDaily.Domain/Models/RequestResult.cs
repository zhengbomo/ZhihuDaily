namespace ZhihuDaily.Domain.Models
{
    public enum RequestResultType
    {
        Cancel,
        Success,
        Error
    }

    public class RequestResult
    {
        public string ErrorMessage { get; set; }
        public RequestResultType ResultType { get; set; }
        public bool IsSuccess => ResultType == RequestResultType.Success;
    }

    public class RequestResult<T> : RequestResult
    {
        public T Content { get; set; }
    }
}
