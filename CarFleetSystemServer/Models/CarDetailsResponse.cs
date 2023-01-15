namespace CarFleetSystemServer.Models;

public class CarDetailsResponse: Response
{
    public CarDetailsResponse(string errorDescription, int errorCode) : base(errorDescription, errorCode){ }

    public CarDetailsResponse() : base("", 0){}
    
    public CarReport Report { get; set; }
}