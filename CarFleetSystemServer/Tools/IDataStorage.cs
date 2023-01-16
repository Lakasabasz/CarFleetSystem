using CarFleetSystemServer.Models;

namespace CarFleetSystemServer.Tools;

public interface IDataStorage
{
    ICollection<UserData> Users { get; }
    ICollection<LoggedInUser> LoggedInUsers { get; }
    ICollection<CarData> Cars { get; }
    ICollection<CarDetails> CarDetails { get; }

    void SaveChanges();
}