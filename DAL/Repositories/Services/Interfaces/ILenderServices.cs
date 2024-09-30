using DAL.DTO.Req;
using DAL.DTO.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services.Interfaces
{
    public interface ILenderServices
    {
        Task<ResBaseDto<ResUserDto>> GetBalance(string lenderId);
        Task<string> UpdateBalance(ReqUpdateBalanceDto reqUpdateBalance);
        Task<List<ResListLoanDto>> GetLoanRequest();
        Task<string> UpdateLoanStatus(string loanId, string newStatus);
        Task<string> ApproveLoan(string loanId, string lenderId);
        Task<List<ResLoanHistoryDto>> GetLoanHistory(string lenderId);
    }
}
