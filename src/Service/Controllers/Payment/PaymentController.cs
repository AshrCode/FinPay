﻿using Application.Payment.Topup;
using Common.ApiException;
using Microsoft.AspNetCore.Mvc;

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
        /// Tops up the specified amount to the beneficiary.
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
                await _topupApp.MakePayment(request.UserID, request.BeneficiaryId, request.Amount);

                // We can also create a generic response model for all the end-points.
                return Ok();
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