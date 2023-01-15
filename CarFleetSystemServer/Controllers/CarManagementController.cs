using CarFleetSystemServer.Models;
using CarFleetSystemServer.Tools;
using Microsoft.AspNetCore.Mvc;

namespace CarFleetSystemServer.Controllers;

[ApiController, Route("api/fleet/[action]")]
public class CarManagementController : Controller
{
    [HttpGet("/")]
    public CarListResponse Index([FromHeader(Name = "Token")] string usertoken) // List all cars
    {
        var user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new CarListResponse("User are not logged in", 100);
        if (!user.User.Permission.ViewCarList)
            return new CarListResponse("User does not have permission to view car list", 101);
        return new CarListResponse()
        {
            Cars = DataStorage.Instance.Cars.ToList()
        };
    }

    [HttpPut]
    public Response AddCar([FromHeader(Name = "Token")] string usertoken, [FromBody] CarData data)
    {
        var user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new Response("User are not logged in", 100);
        if (!user.User.Permission.AddCar)
            return new Response("User does not have permission to add new car", 102);
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