using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Interfaces
{
    public interface IUserServices
    {
        Task<string> Register(ReqRegisterUserDto register);
        Task<ResLoginDto> Login(ReqLoginDto login);
        Task<List<ResUserDto>> GetAllUsers();
        Task<ResUserDto> GetUserById(string id);
        Task DeleteUserById(string id);
        Task<ResUserDto> UpdateUserById(string id, ReqEditUserDto user);
    }
}
