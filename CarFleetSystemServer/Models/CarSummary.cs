namespace CarFleetSystemServer.Models;

public class CarSummary
{
    public double RegisteredDistance { get; set; }
    public bool CurrentClaimed { get; set; }
    public string? LastClaimer { get; set; }
    public string? CurrentClaimer { get; set; }
    public DateTime? LastUpdate { get; set; }
    public double? H { get; set; }
    public double? W { get; set; }
}