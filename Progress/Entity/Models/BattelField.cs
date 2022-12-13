using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models;

[Table("BattelFields", Schema = "data")]
public class BattelField
{
    [Key]
    public long Id { get; set; }
    public long UserId { get; set; }

    #region Navigations

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    #endregion
}