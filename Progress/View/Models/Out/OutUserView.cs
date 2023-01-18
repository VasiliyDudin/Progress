using Entity.Models;

namespace View.Models.Out;

public class OutUserView
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string[] Roles { get; set; }

    public string Token { get; set; }

    public string RefreshToken { get; set; }
}