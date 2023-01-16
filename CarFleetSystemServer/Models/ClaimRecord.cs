namespace CarFleetSystemServer.Models;

public class ClaimRecord
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public UserData Claimer { get; set; }
}