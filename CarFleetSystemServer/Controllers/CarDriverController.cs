using CarFleetSystemServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarFleetSystemServer.Controllers;

[ApiController]
public class CarDriverController : Controller
{
    [HttpGet]
    public JsonResult Index(string usertoken) // List all available cars
    {
        throw new NotImplementedException();
    }
    
    [HttpPost]
    public JsonResult ClaimCar(string usertoken, string cartoken)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public JsonResult UpdateCar(string usertoken, TelemetryData data)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public JsonResult ReleaseCar(string usertoken, string cartoken)
    {
        throw new NotImplementedException();
    }
}