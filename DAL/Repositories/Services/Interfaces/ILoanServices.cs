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
    public interface ILoanService
    {
        Task<ResBaseDto<string>> CreateLoan(ReqLoanUserDto loan);
        Task<ResBaseDto<List<ResListLoanDto>>> GetLoans(string status = null);
        Task<ResBaseDto<List<ResListLoanDto>>> LoanList();
        Task<ResBaseDto<string>> UpdateLoan(string id, ReqUpdateLoanDto reqUpdateLoan);
    }
}
