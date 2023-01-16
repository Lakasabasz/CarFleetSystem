namespace CarFleetSystemServer.Models;

public class CarUpdateRequest
{
    public CarData Data { get; set; }
    public int CarId { get; set; }
}