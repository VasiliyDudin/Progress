using Entity.Models;

namespace View.Models.In;

public class InUserView
{
    
    public long Id { get; set; }
    public string Name { get; set; }
    public string IpAdress { get; set; }
    public string Email { get; set; }
    public Status Status { get; set; }
}