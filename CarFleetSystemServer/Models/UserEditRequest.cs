namespace CarFleetSystemServer.Models;

public class UserEditRequest
{
    public string OriginalUsername { get; set; }
    public UserData Data { get; set; }
}