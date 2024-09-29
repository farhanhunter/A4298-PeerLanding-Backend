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
    public class LenderServices : ILenderServices
    {
        private readonly P2plandingContext _context;

        public LenderServices(P2plandingContext context)
        {
            _context = context;
        }
        public async Task<string> ApproveLoan(string loanId, string lenderId)
        {
            var loan = await _context.MstLoans.FindAsync(loanId);
            var lender = await _context.MstUsers.FindAsync(lenderId);
            var borrower = await _context.MstUsers.FindAsync(loan.BorrowerId);

            if (loan == null || lender == null || borrower == null)
            {
                return "Data tidak ditemukan";
            }

            if (lender.Balance < loan.Amount)
            {
                return "Saldo lender tidak mencukupi";
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                loan.Status = "funded";
                lender.Balance -= loan.Amount;
                borrower.Balance += loan.Amount;

                var funding = new TrnFunding
                {
                    LoanId = loanId,
                    LenderId = lenderId,
                    Amount = loan.Amount,
                    FundedAt = DateTime.UtcNow
                };

                await _context.TrnFundings.AddAsync(funding);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Pinjaman berhasil disetujui";
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return "Terjadi kesalahan saat menyetujui pinjaman";
            }
        }


        public async Task<decimal> GetBalance(string lenderId)
        {
            var lender = await _context.MstUsers.FindAsync(lenderId);
            return lender?.Balance ?? 0;
        }
        public async Task<List<ResLoanHistoryDto>> GetLoanHistory(string lenderId)
        {
            return await _context.TrnFundings
                .Where(f => f.LenderId == lenderId)
                .Select(f => new ResLoanHistoryDto
                {
                    Id = f.Loans.id,
                    BorrowerName = f.Loans.Borrower.Name,
                    Amount = f.Amount,
                    InterestRate = f.Loans.InterestRate,
                    Duration = f.Loans.Duration,
                    Status = f.Loans.Status,
                    CreatedAt = f.Loans.CreatedAt,
                    UpdatedAt = f.Loans.UpdatedAt
                }).ToListAsync();
        }


        public async Task<List<ResListLoanDto>> GetLoanRequest()
        {
            return await _context.MstLoans
                .Where(l => l.Status == "requested")
                .Select(l => new ResListLoanDto
                {
                    LoanId = l.id,
                    BorrowerName = l.Borrower.Name,
                    Amount = l.Amount,
                    InterestRate = l.InterestRate,
                    Duration = l.Duration,
                    Status = l.Status,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt
                }).ToListAsync();
        }


        public async Task<string> UpdateBalance(ReqUpdateBalanceDto reqUpdateBalance)
        {
            var lender = await _context.MstUsers.FindAsync(reqUpdateBalance.LenderId);
            if (lender == null)
            {
                return "Lender tidak ditemukan";
            }

            lender.Balance += reqUpdateBalance.Amount;
            await _context.SaveChangesAsync();
            return "Saldo berhasil diperbarui";
        }

        public async Task<string> UpdateLoanStatus(string loanId, string newStatus)
        {
            var loan = await _context.MstLoans.FindAsync(loanId);
            if (loan == null)
            {
                return "Pinjaman tidak ditemukan";
            }

            loan.Status = newStatus;
            await _context.SaveChangesAsync();
            return "Status pinjaman berhasil diperbarui";
        }

    }
}
