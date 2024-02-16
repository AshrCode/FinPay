namespace Service.Responses
{
    public class ErrorCodeResponse
    {
        public int ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }

        public ErrorCodeResponse(int errcode, string errorMessage)
        {
            ErrorCode = errcode;
            ErrorMessage = errorMessage;
        }
    }
}
