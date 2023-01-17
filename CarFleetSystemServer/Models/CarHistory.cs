using Geolocation;

namespace CarFleetSystemServer.Models;

public class CarHistory
{
    private ICollection<CarPosition> PositionHistory { get; set; }
    private UserData? Claimer { get; set; }
    private DateTime? ClaimedSince { get; set; }
    private DateTime? LastUpdate { get; set; }
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

    public bool IsClaimed() => Claimer is not null;
    public void Claim(UserData user)
    {
        if (Claimer is not null)
            throw new ArgumentException("Claiming failed due to car already claimed");
        Claimer = user;
        ClaimedSince = DateTime.Now;
        LastUpdate = DateTime.Now;
    }

    public bool IsClaimedBy(UserData user) => Claimer != null && Claimer.Username == user.Username;

    public void Update(TelemetryData data)
    {
        LastUpdate = data.Date;
        PositionHistory.Add(new CarPosition(){H = data.H, W = data.W, UpdateTime = data.Date});
    }

    public void Release()
    {
        if (Claimer is null || ClaimedSince is null)
            throw new ArgumentNullException(nameof(Claimer));
        ClaimHistory.Add(new ClaimRecord(){Claimer = Claimer, Start = ClaimedSince.Value, End = DateTime.Now});
        Claimer = null;
        ClaimedSince = null;
    }
}