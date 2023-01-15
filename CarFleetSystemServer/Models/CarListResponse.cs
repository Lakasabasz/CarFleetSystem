namespace CarFleetSystemServer.Models;

public class CarListResponse: Response
{
    public IEnumerable<CarData> Cars { get; set; }

    public CarListResponse(string errorDescription, int errorCode) : base(errorDescription, errorCode){ }

    public CarListResponse() : base("", 0){}
}