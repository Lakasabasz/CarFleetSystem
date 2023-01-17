namespace CarFleetSystemServer.Models;

public class CarDetailsData
{
    public Color CarColor { get; set; }
    public string Description { get; set; }
    public DateOnly NextCarReview { get; set; }
    public DateOnly InsuranceEnding { get; set; }
}