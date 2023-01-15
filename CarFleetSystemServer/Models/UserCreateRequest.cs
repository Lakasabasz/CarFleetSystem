namespace CarFleetSystemServer.Models;

public class UserCreateRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public PermissionSet Permission { get; set; } = null!;
}