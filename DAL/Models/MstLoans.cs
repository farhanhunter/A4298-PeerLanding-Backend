using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [Table("mst_loans")]
    public class MstLoans
    {
        [Key]
        [Column("id")]
        public string id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey("Borrower")]
        [Column("borrower_id")]
        public string BorrowerId { get; set; }

        [Required]
        [Column("amount")]
        public decimal Amount { get; set; }

        [Required]
        [Column("interest_rate")]
        public decimal InterestRate { get; set; }

        [Required]
        [Column("duration_month")]
        public int Duration { get; set; }

        private string _status = "requested";

        [Required]
        [Column("status")]
        public string Status
        {
            get => _status;
            set
            {
                if (value != "requested" && value != "funded" && value != "repaid")
                {
                    throw new ArgumentException("Invalid status value");
                }
                _status = value;
            }
        }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual MstUser Borrower { get; set; }
    }

}
