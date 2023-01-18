using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace View.Models.In;

public class InAuthView
{
    [Required]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; }
    
}