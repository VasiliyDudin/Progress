using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models;

[Table("GameStatistics", Schema = "data")]
public class GameStatistic
{
    [Key]
    public long Id { get; set; }
    public long GameId { get; set; }
    public long UserId { get; set; }
    public bool IsWin { get; set; }

    #region Navigations

    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; }

    
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
    #endregion
}