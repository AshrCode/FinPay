namespace Infrastructure.Models
{
    public class UserAccountModel
    {
        public int ErrorCode { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public string Id { get; set; }
        public int Balance { get; set; }
        public string Currency { get; set; }
    }
}
