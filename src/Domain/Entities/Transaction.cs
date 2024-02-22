using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Domain.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid BeneficiaryId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public float Amount { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public float Fee { get; set; } = 0;

        [IgnoreDataMember]
        public float TotalAmount { get { return Amount + Fee; } }
    }
}
