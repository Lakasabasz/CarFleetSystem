namespace CarFleetSystemServer.Models;

public class UserData
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public PermissionSet Permission { get; set; } = null!;
}