using CarFleetSystemServer.Models;

namespace CarFleetSystemServer.Tools;

public interface IDataStorage
{
    ICollection<UserData> Users { get; }
    ICollection<LoggedInUser> LoggedInUsers { get; }
    IEnumerable<CarData> Cars { get; set; }

    void SaveChanges();
}