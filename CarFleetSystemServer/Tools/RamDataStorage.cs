using CarFleetSystemServer.Models;

namespace CarFleetSystemServer.Tools;

public class RamDataStorage: IDataStorage
{
    public RamDataStorage()
    {
        LoggedInUsers = new List<LoggedInUser>();
        Users = new List<UserData>();
    }

    public ICollection<UserData> Users { get; }
    public ICollection<LoggedInUser> LoggedInUsers { get; }
    public IEnumerable<CarData> Cars { get; set; }

    public void SaveChanges()
    {
        for (int i = 0; i < LoggedInUsers.Count; i++)
        {
            var user = LoggedInUsers.ElementAt(i);
            if (user.LoggedTill < DateTime.Now) LoggedInUsers.Remove(user);
        }
    }
}