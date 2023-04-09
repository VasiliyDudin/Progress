using System.ComponentModel.DataAnnotations;

namespace View.Models.In;

public class InRefreshTokenView
{
    [Required]
    public string Token { get; set; }
}