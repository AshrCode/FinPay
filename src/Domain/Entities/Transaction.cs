using Domain.Enum;
using System.Security.Principal;

namespace Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid BeneficiaryId { get; set; }

        public DateTime TransactionDate { get; set; }

        public TransactionType TransactionType { get; set; }

        public float Amount { get; set; }

        public float Fee { get; set; } = 0;

        public float TotalAmount { get { return Amount + Fee; } }
    }
}
