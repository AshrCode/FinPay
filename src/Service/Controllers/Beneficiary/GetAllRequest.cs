using System.ComponentModel.DataAnnotations;

namespace Service.Controllers.Beneficiary
{
    public class GetAllRequest
    {
        [Required]
        public Guid UserID { get; set; }
    }
}
