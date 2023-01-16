namespace CarFleetSystemServer.Models;

public class CarDetails
{
    public CarDetails(CarData cd)
    {
        if (!cd.Id.HasValue) throw new ArgumentException("CarData.Id");
        CarDataId = cd.Id.Value;
        Details = new();
        History = new();
    }

    public CarDetails()
    {
        Details = new();
        History = new();
    }
    public int CarDataId { get; set; }
    public CarDetailsData Details { get; set; }
    public CarHistory History { get; set; }
}