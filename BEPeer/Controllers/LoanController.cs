using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BEPeer.Controllers
{
    [Route("rest/v1/loan[action]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanServices _loanServices;

        public LoanController(ILoanServices loanServices)
        {
            _loanServices = loanServices;
        }

        [HttpPost]
        public async Task<IActionResult> Register(ReqLoanUserDto reqLoan)
        {
            try
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
                    var errorMessage = new StringBuilder("Validation errors occured!");
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                }

                var res = await _loanServices.CreateLoan(reqLoan);

                return Ok(new ResBaseDto<string>
                {
                    Success = true,
                    Message = res.ToString(),
                    Data = res
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Email already used")
                {
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = ex.Message,
                        Data = null
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        [HttpPut("UpdateStatus/{borrowerId}")]
        public async Task<IActionResult> UpdateLoan(string id, ReqUpdateLoanDto reqUpdateLoan)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Id or Status cannot be null or empty");
            }

            var result = await _loanServices.UpdateLoan(id, reqUpdateLoan);

            if (result == "Loan not found")
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        [HttpGet("LoanList")]
        public async Task<IActionResult> GetLoanList([FromQuery] string status = null)
        {
            var loans = await _loanServices.GetLoans(status);

            if (loans == null || loans.Count == 0)
            {
                return NotFound("No loans found");
            }

            return Ok(new
            {
                success = true,
                message = "Success load loan",
                data = loans
            });
        }
    }
}
