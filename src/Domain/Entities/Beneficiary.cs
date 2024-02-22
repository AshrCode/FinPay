using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Beneficiary
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string NickName { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
