using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdoptionHub.Models;

[Table("loginlogs")]
public class LoginLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    [Column("message")]
    [StringLength(255)]
    public string Message { get; set; } = null!;
}