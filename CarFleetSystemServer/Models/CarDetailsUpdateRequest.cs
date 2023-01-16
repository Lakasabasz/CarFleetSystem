namespace CarFleetSystemServer.Models;

public class CarDetailsUpdateRequest
{
    public int CarId { get; set; }
    public CarDetails Data { get; set; }
}