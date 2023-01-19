using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entity.Models;

[Table("users", Schema = "auth")]
public class User
{
    [Column("id"), Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column("name"), Required]
    public string Name { get; set; }
    
    [Column("ip_adress"), Required]
    public string IpAdress { get; set; }
    
    [Column("email"), Required, EmailAddress]
    public string Email { get; set; }
    
    [Column("status"), Required]
    public Status Status { get; set; }
    
    [Column("roles"), Required]
    public string RolesString
    {
        get => Roles != null ? string.Join(",", Roles) : null;
        set => Roles = !string.IsNullOrWhiteSpace(value) ? value.Split(",") : null;
    }
    
    [NotMapped]
    public string[] Roles { get; set; }
    
    [Column("password_hash"), Required]
    public byte[] PasswordHash { get; set; }

    [Column("password_salt"), Required]
    public byte[] PasswordSalt { get; set; }
}

public enum Status
{
    Active,
    Unactive,
    InGame
}