using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [Table("trn_funding")]
    public class TrnFunding
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey("MstLoans")]
        [Column("loan_id")]
        public string LoanId { get; set; }

        [Required]
        [ForeignKey("MstUser")]
        [Column("lender_id")]
        public string LenderId { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [Required]
        [Column("funded_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public MstUser User { get; set; }

        public MstLoans Loans { get; set; }
    }
}
