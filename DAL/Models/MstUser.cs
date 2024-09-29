using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

[Table("mst_users")]
public partial class MstUser
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    [Column("email")]
    public string Email { get; set; } = null!;

    [Required]
    [Column("password")]
    public string Password { get; set; } = null!;

    [Required]
    [Column("role")]
    public string Role { get; set; } = null!;

    [Column("balance", TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [InverseProperty("Borrower")]
    public virtual ICollection<MstLoans> BorrowedLoans { get; set; } = new List<MstLoans>();
}

