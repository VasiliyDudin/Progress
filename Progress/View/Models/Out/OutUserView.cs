using Entity.Models;

namespace View.Models.Out;

public class OutUserView
{
    
    public long Id { get; set; }
    public string Name { get; set; }
    public string IpAdress { get; set; }
    public string Email { get; set; }
    public Status Status { get; set; }
}