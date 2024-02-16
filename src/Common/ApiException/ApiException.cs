namespace Common.ApiException
{
    public class ApiException : Exception
    {
        public int ErrorCode { get; private set; }

        public ApiException(int errorCode, string errorMessage)
        : base(errorMessage) 
        { 
            ErrorCode = errorCode;
        }
    }
}
