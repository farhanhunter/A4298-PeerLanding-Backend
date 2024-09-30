using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BEPeer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "lender")]
    public class LenderController : ControllerBase
    {
        private readonly ILenderServices _lenderServices;
        private readonly ILogger<LenderController> _logger;

        public LenderController(ILenderServices lenderServices, ILogger<LenderController>logger)
        {
            _lenderServices = lenderServices;
            _logger = logger;
        }

        private string GetLenderId()
        {
            _logger.LogInformation("Entering GetLenderId method");

            if (User == null || !User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("User is null or not authenticated");
                return null;
            }

            _logger.LogInformation("User claims:");
            foreach (var claim in User.Claims)
            {
                _logger.LogInformation($"Type: {claim.Type}, Value: {claim.Value}");
            }

            var lenderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation($"Extracted Lender ID from token: {lenderId ?? "null"}");

            if (string.IsNullOrEmpty(lenderId))
            {
                _logger.LogWarning("Lender ID not found in claims");
            }

            return lenderId;
        }


        [HttpGet("balance")]
        [Authorize(Roles = "Lender")]
        public async Task<ActionResult<ResBaseDto<ResUserDto>>> GetBalance()
        {
            _logger.LogInformation("Entering GetBalance method");

            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            _logger.LogInformation($"Received token: {token}");

            var lenderId = GetLenderId();
            _logger.LogInformation($"Lender ID from token: {lenderId ?? "null"}");
            if (string.IsNullOrEmpty(lenderId))
            {
                _logger.LogWarning("Lender ID is null or empty");
                return Unauthorized(new ResBaseDto<ResUserDto>
                {
                    Success = false,
                    Message = "Unauthorized access",
                    Data = null,
                    StatusCode = 401
                });
            }
            var result = await _lenderServices.GetBalance(lenderId);
            _logger.LogInformation($"GetBalance result: {result.Success}, Message: {result.Message}");

            if (result.Success)
            {
                var response = new ResBaseDto<ResUserDto>
                {
                    Success = true,
                    Message = "Balance retrieved successfully",
                    Data = new ResUserDto { Balance = result.Data.Balance },
                    StatusCode = 200
                };
                return Ok(response);
            }
            else
            {
                var statusCode = result.StatusCode != 0 ? result.StatusCode : 400;
                var response = new ResBaseDto<ResUserDto>
                {
                    Success = false,
                    Message = result.Message,
                    Data = null,
                    StatusCode = statusCode
                };
                return StatusCode(statusCode, response);
            }
        }

        [HttpPost("balance/update")]
        public async Task<ActionResult<string>> UpdateBalance([FromBody] ReqUpdateBalanceDto reqUpdateBalanceDto)
        {
            var lenderId = GetLenderId();
            if (string.IsNullOrEmpty(lenderId))
            {
                return Unauthorized();
            }

            if (reqUpdateBalanceDto.LenderId != lenderId)
            {
                return Forbid();
            }

            var result = await _lenderServices.UpdateBalance(reqUpdateBalanceDto);
            return Ok(result);
        }

        [HttpGet("loan-request")]
        public async Task<ActionResult<List<ResListLoanDto>>> GetLoanRequest()
        {
            var lenderId = GetLenderId();
            if (string.IsNullOrEmpty(lenderId))
            {
                return Unauthorized();
            }
            var loanRequest = await _lenderServices.GetLoanRequest();
            return Ok(loanRequest);
        }

        [HttpPost("approve-loan")]
        public async Task<ActionResult<string>> ApproveLoan(string loanId)
        {
            var lenderId = GetLenderId();
            if (string.IsNullOrEmpty(lenderId))
            {
                return Unauthorized();
            }

            var result = await _lenderServices.ApproveLoan(loanId, lenderId);
            return Ok(result);
        }

        [HttpGet("loan-history")]
        public async Task<ActionResult<List<ResLoanHistoryDto>>> GetLoanHistory()
        {
            var lenderId = GetLenderId();
            if (string.IsNullOrEmpty(lenderId))
            {
                return Unauthorized();
            }

            var loanHistory = await _lenderServices.GetLoanHistory(lenderId);
            return Ok(loanHistory);
        }

        [HttpPut("update-loan-status")]
        public async Task<ActionResult<string>> UpdateLoanStatus(string loanId, string newStatus)
        {
            var lenderId = GetLenderId();
            if (string.IsNullOrEmpty(lenderId))
            {
                return Unauthorized();
            }

            var result = await _lenderServices.UpdateLoanStatus(loanId, newStatus);
            return Ok(result);
        }
    }
}
