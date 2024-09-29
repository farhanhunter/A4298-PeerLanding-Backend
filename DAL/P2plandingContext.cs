using System;
using System.Collections.Generic;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public partial class P2plandingContext : DbContext
{
    public P2plandingContext()
    {
    }

    public P2plandingContext(DbContextOptions<P2plandingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MstUser> MstUsers { get; set; }
    public virtual DbSet<MstLoans> MstLoans { get; set; }
    public virtual DbSet<TrnFunding> TrnFundings { get; set; }
    public virtual DbSet<TrnRepayment> TrnRepayments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MstUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mst_users_pkey");

            entity.ToTable("mst_users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Balance)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("balance");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(150)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(30)
                .HasColumnName("role");
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");

            entity.HasMany(e => e.BorrowedLoans)
                .WithOne(l => l.Borrower)
                .HasForeignKey(l => l.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MstLoans>(entity =>
        {
            entity.HasKey(e => e.id).HasName("mst_loans_pkey");

            entity.ToTable("mst_loans");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.BorrowerId).HasColumnName("borrower_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("amount");
            entity.Property(e => e.InterestRate)
                .HasColumnType("decimal(5,2)")
                .HasColumnName("interest_rate");
            entity.Property(e => e.Duration)
                .HasColumnName("duration_month");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasColumnName("status");
            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Borrower)
                .WithMany(p => p.BorrowedLoans)
                .HasForeignKey(d => d.BorrowerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_mst_loans_borrower");
        });

        modelBuilder.Entity<TrnFunding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trn_funding_pkey");

            entity.ToTable("trn_funding");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LoanId).HasColumnName("loan_id");
            entity.Property(e => e.LenderId).HasColumnName("lender_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("amount");
            entity.Property(e => e.FundedAt)
                .HasColumnName("funded_at");

            entity.HasOne(d => d.Loans)
                .WithMany()
                .HasForeignKey(d => d.LoanId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_trn_funding_loan");

            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.LenderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_trn_funding_user");
        });

        modelBuilder.Entity<TrnRepayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trn_repayment_pkey");

            entity.ToTable("trn_repayment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LoanId).HasColumnName("loan_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("amount");
            entity.Property(e => e.RepaidAmount)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("repaid_amount");
            entity.Property(e => e.BalanceAmount)
                .HasColumnType("decimal(18,2)")
                .HasColumnName("balance_amount");
            entity.Property(e => e.RepaidStatus)
                .HasMaxLength(30)
                .HasColumnName("repaid_status");
            entity.Property(e => e.PaidAt)
                .HasColumnName("paid_at");

            entity.HasOne(d => d.Loan)
                .WithMany()
                .HasForeignKey(d => d.LoanId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_trn_repayment_loan");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);



}
