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

        public LenderController(ILenderServices lenderServices)
        {
            _lenderServices = lenderServices;
        }

        private string GetLenderId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        [HttpGet("balance")]
        public async Task<ActionResult<decimal>> GetBalance()
        {
            var lenderId = GetLenderId();
            if (string.IsNullOrEmpty(lenderId))
            {
                return Unauthorized();
            }

            var balance = await _lenderServices.GetBalance(lenderId);
            return Ok(balance);
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
