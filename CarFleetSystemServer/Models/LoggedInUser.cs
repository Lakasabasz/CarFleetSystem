namespace CarFleetSystemServer.Models;

public class LoggedInUser
{
    public UserData User { get; }
    public string UserToken { get; }
    public DateTime LoggedTill { get; private set; }


    public LoggedInUser(UserData user)
    {
        User = user;
        UserToken = BCrypt.Net.BCrypt.EnhancedHashPassword($"{DateTime.Now.Ticks}");
        LoggedTill = DateTime.Now.AddMinutes(15);
    }
    
    public void Prolong()
    {
        LoggedTill = LoggedTill.AddMinutes(15);
    }
}