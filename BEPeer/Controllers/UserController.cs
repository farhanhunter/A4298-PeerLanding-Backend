using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Repositories.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BEPeer.Controllers
{
    [Route("rest/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(ReqRegisterUserDto register)
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
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = "Validation error occurred!",
                        Data = errors
                    });
                }
                if (register.Role.ToLower() == "admin")
                {
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = "You can't register as admin",
                        Data = null
                    });
                }

                var userName = await _userServices.Register(register);
                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = userName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during registration: {ex}");
                var statusCode = (ex is ResErrorDto resError) ? resError.StatusCode : StatusCodes.Status500InternalServerError;
                var message = (ex is ResErrorDto) ? ex.Message : "An unexpected error occurred";

                return StatusCode(statusCode, new ResBaseDto<string>
                {
                    Success = false,
                    Message = message,
                    Data = null
                });
            }
        }




        [Authorize(Roles = "admin")]
        [Route("add-user")]
        [HttpPost]
        public async Task<IActionResult> AddUser(ReqRegisterUserDto register)
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
                    var errorMessage = new StringBuilder("Validation error occurred!");
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                };
                var userName = await _userServices.Register(register);
                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = userName
                });
            }
            catch (ResErrorDto ex)
            {
                return StatusCode(ex.StatusCode, new ResBaseDto<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [Authorize(Roles = "admin")]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userServices.GetAllUsers();
                return Ok(new ResBaseDto<List<ResUserDto>>
                {
                    Success = true,
                    Message = "Users fetched successfully",
                    Data = users
                });
            }
            catch (ResErrorDto ex)
            {
                return StatusCode(ex.StatusCode, new ResBaseDto<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(ReqLoginDto login)
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
                    var errorMessage = new StringBuilder("Validation error occurred!");
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                };
                var user = await _userServices.Login(login);
                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User logged in successfully",
                    Data = user
                });
            }
            catch (ResErrorDto ex)
            {
                return StatusCode(ex.StatusCode, new ResBaseDto<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = "An unexpected error occurred. Please try again later.",
                    Data = null
                });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userServices.DeleteUserById(id);
                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User deleted successfully",
                    Data = null
                });
            }
            catch (ResErrorDto ex)
            {
                return StatusCode(ex.StatusCode, new ResBaseDto<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = "An unexpected error occurred. Please try again later.",
                    Data = null
                });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(string id, ReqEditUserDto dto)
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
                    var errorMessage = new StringBuilder("Validation error occurred!");
                    return BadRequest(new ResBaseDto<object>
                    {
                        Success = false,
                        Message = errorMessage.ToString(),
                        Data = errors
                    });
                };
                var user = await _userServices.UpdateUserById(id, dto);
                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User updated successfully",
                    Data = user
                });
            }
            catch (ResErrorDto ex)
            {
                return StatusCode(ex.StatusCode, new ResBaseDto<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = "An unexpected error occurred. Please try again later.",
                    Data = null
                });
            }
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _userServices.GetUserById(id);
                return Ok(new ResBaseDto<object>
                {
                    Success = true,
                    Message = "User fetched successfully",
                    Data = user
                });
            }
            catch (ResErrorDto ex)
            {
                return StatusCode(ex.StatusCode, new ResBaseDto<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResBaseDto<string>
                {
                    Success = false,
                    Message = "An unexpected error occurred. Please try again later.",
                    Data = null
                });
            }
        }
    }
}
