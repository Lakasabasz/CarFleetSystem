namespace CarFleetSystemServer.Models;

public class AvailableCarsResponse: Response
{
    public AvailableCarsResponse(string errorDescription, int errorCode) : base(errorDescription, errorCode){}

    public AvailableCarsResponse() : base("", 0){}
    
    public IEnumerable<CarData> Cars { get; set; }
}