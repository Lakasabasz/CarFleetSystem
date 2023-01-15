using Microsoft.AspNetCore.Mvc;

namespace CarFleetSystemServer.Controllers;

[Route("/")]
public class HelloController : Controller
{
    [HttpGet("")]
    public string Index()
    {
        return $"Hello, current time is: {DateTime.Now}";
    }
    
    [HttpGet("dupa")]
    public string Dupa()
    {
        return $"Dupa";
    }
}