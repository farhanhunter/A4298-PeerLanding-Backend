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
    public interface ILoanServices
    {
        Task<string> CreateLoan(ReqLoanUserDto loan);

        Task<string> UpdateLoan(string borrowerId, ReqUpdateLoanDto reqUpdateLoan);

        Task<List<ResListLoanDto>> GetLoans(string status = null);
        Task<List<ResListLoanDto>> LoanList();
    }
}
