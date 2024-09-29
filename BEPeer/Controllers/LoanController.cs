using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BEPeer.Controllers
{
    [Route("rest/v1/loan")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(ReqLoanUserDto reqLoan)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Any())
                    .Select(x => new
                    {
                        Field = x.Key,
                        Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    }).ToList();
                var errorMessage = new StringBuilder("Validation errors occurred!");
                return BadRequest(new ResBaseDto<object>
                {
                    Success = false,
                    Message = errorMessage.ToString(),
                    Data = errors
                });
            }

            var result = await _loanService.CreateLoan(reqLoan);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateLoan(string id, ReqUpdateLoanDto reqUpdateLoan)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ResBaseDto<string>
                {
                    Success = false,
                    Message = "Id cannot be null or empty",
                    Data = null
                });
            }

            var result = await _loanService.UpdateLoan(id, reqUpdateLoan);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetLoanList([FromQuery] string status = null)
        {
            var result = await _loanService.GetLoans(status);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllLoans()
        {
            var result = await _loanService.LoanList();

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
