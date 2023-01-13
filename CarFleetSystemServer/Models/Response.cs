namespace CarFleetSystemServer.Models;

public class Response
{
    public Response(string errorDescription, int errorCode)
    {
        ErrorDescription = errorDescription;
        ErrorCode = errorCode;
    }

    public string ErrorDescription { get; set; }
    public int ErrorCode { get; set; }
}