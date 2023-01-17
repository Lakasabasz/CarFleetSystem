namespace CarFleetSystemServer.Models;

public class CarDetailsUpdateRequest
{
    public int CarId { get; set; }
    public CarDetailsData Data { get; set; }
}