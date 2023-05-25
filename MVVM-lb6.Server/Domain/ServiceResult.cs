namespace Server.Domain
{
    public class ServiceResult
    {
        public bool IsSuccessful { get; set; }
        public string[]? Message { get; set; }

        public ServiceResult()
        {
            IsSuccessful = true;
            Message = null;
        }
        public ServiceResult(params string[] errorMessage)
        {
            IsSuccessful = false;
            Message = errorMessage;
        }
    }
    
    public class ServiceResult<T> : ServiceResult
    {
        public T? Value { get; set; }

        public ServiceResult(T value) : base()
        {
            Value = value;
        }
        public ServiceResult(string message) : base(message)
        {
            Value = default(T);
        }
    }
}
