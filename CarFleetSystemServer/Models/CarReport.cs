namespace CarFleetSystemServer.Models;

public class CarReport // Data From Endpoint Get Car Details
{
    public CarDetailsData Details { get; set; }
    public CarSummary Summary { get; set; }
}