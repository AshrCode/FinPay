using Application.Beneficiary;
using Common.ApiException;
using Microsoft.AspNetCore.Mvc;
using Service.Responses;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Service.Controllers.Beneficiary
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiaryController : BaseController
    {
        private readonly IBeneficiaryApp _beneficiaryApp;

        public BeneficiaryController(ILogger<BeneficiaryController> logger, IBeneficiaryApp beneficiaryApp)
            : base(logger)
        {
            _beneficiaryApp = beneficiaryApp;
        }

        /// <summary>
        /// Adds a new beneficiary for the specified user.
        /// </summary>
        // POST api/<BeneficiaryController>/Add
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add(AddRequest request)
        {
            try
            {
                var result = await _beneficiaryApp.CreateAsync(request.NickName,request.UserId, request.IsActive);

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

        /// <summary>
        /// Gets all the beneficiaries for the specified user.
        /// </summary>
        // Get api/<BeneficiaryController>/GetAllActive/B136CF3D-766B-45AE-AA84-AC7F10C5A090
        [HttpGet("GetAllActive/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllActive(Guid userId)
        {
            try
            {
                var result = await _beneficiaryApp.GetAllAsync(userId, true);

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
