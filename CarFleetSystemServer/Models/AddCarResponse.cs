namespace CarFleetSystemServer.Models;

public class AddCarResponse: Response
{
    public int CreatedId { get; set; }
    
    public AddCarResponse(string errorDescription, int errorCode) : base(errorDescription, errorCode)
    {
    }

    public AddCarResponse() : base("", 0){}
}