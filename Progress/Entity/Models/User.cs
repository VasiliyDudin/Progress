using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models;

[Table("Users", Schema = "data")]
public class User
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public string IpAdress { get; set; }
    public string Email { get; set; }
    public Status Status { get; set; }
}

public enum Status
{
    Active,
    Unactive,
    InGame
}