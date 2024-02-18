using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Service.Controllers.Payment
{
    public class TopupRequest
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        public Guid BeneficiaryId { get; set; }

        [Required]
        [Range(5, 1000, ErrorMessage = "The minimum amount should be 5 AED")]
        public float Amount { get; set; }
    }
}
