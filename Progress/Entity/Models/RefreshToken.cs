using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models;

[Table("refresh_tokens", Schema = "auth")]
public class RefreshToken
{
    [Column("id")]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Column("token"), Required]
    public string Token { get; set; }

    [Column("user_id"), Required]
    public long UserId { get; set; }

    [Column("expiration"), Required]
    public DateTimeOffset Expiration { get; set; }

    [ForeignKey("user_id")]
    public User User { get; set; }
}