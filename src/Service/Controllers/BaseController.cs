using Common.ApiException;
using Microsoft.AspNetCore.Mvc;
using Service.Responses;

namespace Service.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;
        private const string commonErrorMsg = "An error occured. Please contact system administrator.";

        protected BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles API exceptions.
        /// </summary>
        protected IActionResult HandleApiException(ApiException ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);

            ErrorCodeResponse errorModel = new(ex.ErrorCode, ex.Message);

            return ex.ErrorCode switch
            {
                ApiErrorCodes.BadRequest => BadRequest(errorModel),
                ApiErrorCodes.Conflict => Conflict(errorModel),
                ApiErrorCodes.Unauthorized => Unauthorized(),
                ApiErrorCodes.Forbidden => Forbid(),
                ApiErrorCodes.NotFound => NotFound(errorModel),
                _ => StatusCode(ApiErrorCodes.InternalError, errorModel),
            };
        }

        /// <summary>
        /// Handles uncategorized exceptions.
        /// </summary>
        protected IActionResult HandleApiException(Exception ex)
        {
            _logger.LogError(ex.Message, ex.StackTrace);

            ErrorCodeResponse errorModel = new(ApiErrorCodes.InternalError, commonErrorMsg);
            return StatusCode(ApiErrorCodes.InternalError, errorModel);
        }
    }
}
