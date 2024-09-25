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
    public class LoanServices : ILoanServices
    {
        private readonly P2plandingContext _p2PlandingContext;

        public LoanServices(P2plandingContext p2PlandingContext)
        {
            _p2PlandingContext = p2PlandingContext;
        }

        public async Task<string> CreateLoan(ReqLoanUserDto loan)
        {
            var newLoan = new MstLoans
            {
                id = loan.id,
                Amount = loan.Amount,
                InterestRate = loan.InterestRate,
                Duration = loan.Duration,
                Status = "requested" // Default status untuk pinjaman baru
            };

            await _p2PlandingContext.AddAsync(newLoan);
            await _p2PlandingContext.SaveChangesAsync();

            return newLoan.id;
        }

        public async Task<List<ResListLoanDto>> GetLoans(string status = null)
        {
            var loans = _p2PlandingContext.MstLoans.AsQueryable();

            // Jika status tidak kosong, filter berdasarkan status
            if (!string.IsNullOrEmpty(status))
            {
                loans = loans.Where(l => l.Status == status);
            }

            return await loans.Select(l => new ResListLoanDto
            {
                LoanId = l.id,
                BorrowerName = l.User.Name, // Asumsi Anda memiliki relasi ke tabel User
                Amount = l.Amount,
                InterestRate = l.InterestRate,
                Duration = l.Duration,
                Status = l.Status,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            }).ToListAsync();
        }

        public async Task<List<ResListLoanDto>> LoanList()
        {
            var loans = await _p2PlandingContext.MstLoans
                .Include(l => l.User) // Memasukkan data User dari relasi
                .Select(loan => new ResListLoanDto
                {
                    LoanId = loan.id,
                    BorrowerName = loan.User.Name, // Mengambil nama peminjam dari User
                    Amount = loan.Amount,
                    InterestRate = loan.InterestRate,
                    Duration = loan.Duration,
                    Status = loan.Status,
                    CreatedAt = loan.CreatedAt,
                    UpdatedAt = loan.UpdatedAt
                }).ToListAsync();

            return loans;
        }

        public async Task<string> UpdateLoan(string id, ReqUpdateLoanDto reqUpdateLoan)
        {
            var loans = await _p2PlandingContext.MstLoans.FirstOrDefaultAsync(l => l.id == id);

            if (loans == null)
            {
                return "Loan tidak ditemukan";
            }

            loans.Status = reqUpdateLoan.status;

            _p2PlandingContext.MstLoans.Update(loans);
            await _p2PlandingContext.SaveChangesAsync();

            return "Loan status updated successfully";
        }
    }
}