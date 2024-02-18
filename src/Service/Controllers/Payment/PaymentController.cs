using Application.Payment.Topup;
using Common.ApiException;
using Microsoft.AspNetCore.Mvc;
using Service.Responses;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Service.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly ITopupApp _topupApp;

        public PaymentController(ILogger<PaymentController> logger, ITopupApp topupApp) 
            : base(logger)
        {
            _topupApp = topupApp;
        }

        /// <summary>
        /// Tops up the specified amount to the specified beneficiary.
        /// </summary>
        // POST api/<PaymentController>/Topup
        [HttpPost("Topup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Topup(TopupRequest request)
        {
            try
            {
                var result = await _topupApp.MakePaymentAsync(request.UserID, request.BeneficiaryId, request.Amount);

                ApiResponse response = new()
                {
                    ErrorCode = HttpStatusCode.OK,
                    Data = result
                };

                return Ok(response);
            }
            catch (ApiException aexp)
            {
                return HandleApiException(aexp);
            }
            catch (Exception ex)
            {
                return HandleApiException(ex);
            }
        }
    }
}
