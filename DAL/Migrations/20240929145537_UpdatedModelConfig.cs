using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModelConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mst_users",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    role = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mst_users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mst_loans",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    borrower_id = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    interest_rate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    duration_month = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mst_loans_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_mst_loans_borrower",
                        column: x => x.borrower_id,
                        principalTable: "mst_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trn_funding",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    loan_id = table.Column<string>(type: "text", nullable: false),
                    lender_id = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    funded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("trn_funding_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_trn_funding_loan",
                        column: x => x.loan_id,
                        principalTable: "mst_loans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_trn_funding_user",
                        column: x => x.lender_id,
                        principalTable: "mst_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "trn_repayment",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    loan_id = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    repaid_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    balance_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    repaid_status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("trn_repayment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_trn_repayment_loan",
                        column: x => x.loan_id,
                        principalTable: "mst_loans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mst_loans_borrower_id",
                table: "mst_loans",
                column: "borrower_id");

            migrationBuilder.CreateIndex(
                name: "IX_trn_funding_lender_id",
                table: "trn_funding",
                column: "lender_id");

            migrationBuilder.CreateIndex(
                name: "IX_trn_funding_loan_id",
                table: "trn_funding",
                column: "loan_id");

            migrationBuilder.CreateIndex(
                name: "IX_trn_repayment_loan_id",
                table: "trn_repayment",
                column: "loan_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trn_funding");

            migrationBuilder.DropTable(
                name: "trn_repayment");

            migrationBuilder.DropTable(
                name: "mst_loans");

            migrationBuilder.DropTable(
                name: "mst_users");
        }
    }
}
