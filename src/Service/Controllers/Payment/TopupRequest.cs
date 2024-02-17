using System.ComponentModel.DataAnnotations;

namespace Service.Controllers.Payment
{
    public class TopupRequest
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public int BeneficiaryId { get; set; }

        [Required]
        public int Amount { get; set; }
    }
}
