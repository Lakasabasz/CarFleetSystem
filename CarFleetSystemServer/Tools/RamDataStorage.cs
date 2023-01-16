using CarFleetSystemServer.Models;

namespace CarFleetSystemServer.Tools;

public class RamDataStorage: IDataStorage
{
    public RamDataStorage()
    {
        LoggedInUsers = new List<LoggedInUser>();
        Users = new List<UserData>();
        Cars = new List<CarData>();
        CarDetails = new List<CarDetails>();
    }

    public ICollection<UserData> Users { get; }
    public ICollection<LoggedInUser> LoggedInUsers { get; }
    public ICollection<CarData> Cars { get; }
    public ICollection<CarDetails> CarDetails { get; }

    public void SaveChanges()
    {
        for (int i = 0; i < LoggedInUsers.Count; i++)
        {
            var user = LoggedInUsers.ElementAt(i);
            if (user.LoggedTill < DateTime.Now) LoggedInUsers.Remove(user);
        }
    }
}