using DAL.DTO.Req;
using DAL.DTO.Res;
using DAL.Models;
using DAL.Repositories.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Services
{
    public class LoanServices : ILoanService
    {
        private readonly P2plandingContext _p2PlandingContext;

        public LoanServices(P2plandingContext p2PlandingContext)
        {
            _p2PlandingContext = p2PlandingContext;
        }
        public async Task<ResBaseDto<string>> CreateLoan(ReqLoanUserDto loan)
        {
            var response = new ResBaseDto<string>();

            try
            {
                var newLoan = new MstLoans
                {
                    id = Guid.NewGuid().ToString(),
                    BorrowerId = loan.BorrowerId,
                    Amount = loan.Amount,
                    InterestRate = loan.InterestRate,
                    Duration = loan.Duration,
                    Status = "requested",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _p2PlandingContext.MstLoans.AddAsync(newLoan);
                await _p2PlandingContext.SaveChangesAsync();

                response.Success = true;
                response.SetData(newLoan.id);
                response.SetMessage("Loan created successfully");
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.SetMessage($"Error creating loan: {ex.Message}");
            }

            return response;
        }

        public async Task<ResBaseDto<List<ResListLoanDto>>> GetLoans(string status = null)
        {
            var response = new ResBaseDto<List<ResListLoanDto>>();

            try
            {
                var loans = _p2PlandingContext.MstLoans.AsQueryable();

                if (!string.IsNullOrEmpty(status))
                {
                    loans = loans.Where(l => l.Status == status);
                }

                var loanList = await loans
                    .Include(l => l.Borrower)
                    .Select(l => new ResListLoanDto
                    {
                        LoanId = l.id,
                        BorrowerId = l.BorrowerId,
                        BorrowerName = l.Borrower.Name,
                        Amount = l.Amount,
                        InterestRate = l.InterestRate,
                        Duration = l.Duration,
                        Status = l.Status,
                        CreatedAt = l.CreatedAt,
                        UpdatedAt = l.UpdatedAt
                    }).ToListAsync();

                response.Success = true;
                response.SetData(loanList);
                response.SetMessage("Loans retrieved successfully");
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.SetMessage($"Error retrieving loans: {ex.Message}");
            }

            return response;
        }


        public async Task<ResBaseDto<List<ResListLoanDto>>> LoanList()
        {
            var response = new ResBaseDto<List<ResListLoanDto>>();

            try
            {
                var loanList = await _p2PlandingContext.MstLoans
                    .Include(l => l.Borrower)
                    .Select(loan => new ResListLoanDto
                    {
                        LoanId = loan.id,
                        BorrowerId = loan.BorrowerId,
                        BorrowerName = loan.Borrower.Name,
                        Amount = loan.Amount,
                        InterestRate = loan.InterestRate,
                        Duration = loan.Duration,
                        Status = loan.Status,
                        CreatedAt = loan.CreatedAt,
                        UpdatedAt = loan.UpdatedAt
                    }).ToListAsync();

                response.Success = true;
                response.SetData(loanList);
                response.SetMessage("Loan list retrieved successfully");
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.SetMessage($"Error retrieving loan list: {ex.Message}");
            }

            return response;
        }

        public async Task<ResBaseDto<string>> UpdateLoan(string id, ReqUpdateLoanDto reqUpdateLoan)
        {
            var response = new ResBaseDto<string>();

            try
            {
                var loan = await _p2PlandingContext.MstLoans.FirstOrDefaultAsync(l => l.id == id);

                if (loan == null)
                {
                    response.Success = false;
                    response.SetMessage("Loan not found");
                    return response;
                }

                loan.Status = reqUpdateLoan.Status;
                loan.UpdatedAt = DateTime.UtcNow;

                if (reqUpdateLoan.Amount.HasValue)
                    loan.Amount = reqUpdateLoan.Amount.Value;

                if (reqUpdateLoan.InterestRate.HasValue)
                    loan.InterestRate = reqUpdateLoan.InterestRate.Value;

                if (reqUpdateLoan.Duration.HasValue)
                    loan.Duration = reqUpdateLoan.Duration.Value;

                _p2PlandingContext.MstLoans.Update(loan);
                await _p2PlandingContext.SaveChangesAsync();

                response.Success = true;
                response.SetData(loan.id);
                response.SetMessage("Loan updated successfully");
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.SetMessage($"Error updating loan: {ex.Message}");
            }

            return response;
        }
    }
}
