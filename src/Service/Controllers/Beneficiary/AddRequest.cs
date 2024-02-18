using System.ComponentModel.DataAnnotations;

namespace Service.Controllers.Beneficiary
{
    public class AddRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string NickName { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
