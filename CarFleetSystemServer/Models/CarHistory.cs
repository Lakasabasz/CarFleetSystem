using Geolocation;

namespace CarFleetSystemServer.Models;

public class CarHistory
{
    private ICollection<CarPosition> PositionHistory { get; set; }
    private UserData? Claimer { get; set; }
    private DateTime? ClaimedSince { get; set; }
    private ICollection<ClaimRecord> ClaimHistory { get; set; }

    public CarHistory()
    {
        PositionHistory = new List<CarPosition>();
        ClaimHistory = new List<ClaimRecord>();
    }
    public CarSummary GetSummary()
    {
        double distance = 0;
        for (int i = 0; i < PositionHistory.Count - 1; i++)
        {
            CarPosition a = PositionHistory.ElementAt(i);
            CarPosition b = PositionHistory.ElementAt(i + 1);
            distance += GeoCalculator.GetDistance(
                a.W, a.H, b.W, b.H, 3, DistanceUnit.Kilometers);
        }
        return new CarSummary()
        {
            CurrentClaimed = Claimer is not null,
            CurrentClaimer = Claimer?.Username,
            LastClaimer = ClaimHistory.LastOrDefault()?.Claimer.Username,
            LastUpdate = PositionHistory.LastOrDefault()?.UpdateTime,
            H = PositionHistory.LastOrDefault()?.H,
            W = PositionHistory.LastOrDefault()?.W,
            RegisteredDistance = distance
        };
    }
}