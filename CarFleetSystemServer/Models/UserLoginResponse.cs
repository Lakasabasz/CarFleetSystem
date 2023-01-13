namespace CarFleetSystemServer.Models;

public class UserLoginResponse : Response
{
    public UserLoginResponse(string errorDescription, int errorCode) : base(errorDescription, errorCode){ }
    public UserLoginResponse() : base("", 0){ }

    public string? UserToken { get; set; } = null;
}