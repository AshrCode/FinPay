using System.Net;

namespace Service.Responses
{
    public class ApiResponse
    {
        public HttpStatusCode ErrorCode { get; set; }
        public object Data { get; set; }
    }
}
