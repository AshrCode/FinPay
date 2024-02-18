namespace Domain.Entities
{
    public class Beneficiary
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string NickName { get; set; }

        public bool IsActive { get; set; }
    }
}
