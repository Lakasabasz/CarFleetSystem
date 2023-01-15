namespace CarFleetSystemServer.Models;

public class UserData
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public PermissionSet Permission { get; set; } = null!;

    public static UserData From(UserCreateRequest data)
    {
        return new UserData()
        {
            Username = data.Username,
            Password = BCrypt.Net.BCrypt.EnhancedHashPassword(data.Username),
            Permission = data.Permission
        };
    }
}