using CarFleetSystemServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarFleetSystemServer.Controllers;

[ApiController, Route("api/fleet/[action]")]
public class CarManagementController : Controller
{
    [HttpGet]
    public JsonResult Index(string usertoken) // List all cars
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    public JsonResult AddCar(string usertoken, CarData data)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public JsonResult UpdateCar(string usertoken, string cartoken, CarData data)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public JsonResult DeleteCar(string usertoken, string cartoken)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public JsonResult GetCarDetails(string usertoken, string cartoken)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public JsonResult UpdateCarDetails(string usertoken, string cartoken, CarDetails data)
    {
        throw new NotImplementedException();
    }
}