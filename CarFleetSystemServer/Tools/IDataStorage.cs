using CarFleetSystemServer.Models;

namespace CarFleetSystemServer.Tools;

public interface IDataStorage
{
    ICollection<UserData> Users { get; }
    ICollection<LoggedInUser> LoggedInUsers { get; }

    void SaveChanges();
}