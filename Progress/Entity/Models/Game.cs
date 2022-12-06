using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Entity.Models;

[Table("Games", Schema = "data")]
public class Game
{
    [Key]
    public long Id { get; set; }
    public DateTime StartGame { get; set; }
    public bool IsGameOver { get; set; }
}