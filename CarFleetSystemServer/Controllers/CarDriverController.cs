using CarFleetSystemServer.Models;
using CarFleetSystemServer.Tools;
using Microsoft.AspNetCore.Mvc;

namespace CarFleetSystemServer.Controllers;

[ApiController, Route("api/car/[action]")]
public class CarDriverController : Controller
{
    [HttpGet]
    public AvailableCarsResponse List([FromHeader(Name = "Token")] string usertoken) // List all available cars
    {
        var user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new AvailableCarsResponse("User are not logged in", 200);
        if (!user.User.Permission.ViewAvailableCarList)
            return new AvailableCarsResponse("User does not have permission to view available cars list", 201);
        int[] available = DataStorage.Instance.CarDetails.Where(x => !x.History.IsClaimed())
            .Select(x => x.CarDataId).ToArray();
        return new AvailableCarsResponse()
        {
            Cars = DataStorage.Instance.Cars.Where(x => x.Id is not null && available.Contains(x.Id.Value))
        };
    }
    
    [HttpPost]
    public Response ClaimCar([FromHeader(Name = "Token")] string usertoken, [FromBody] CarClaimRequest claimRequest)
    {
        var user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new Response("User are not logged in", 200);
        if (!user.User.Permission.ClaimCar)
            return new Response("User does not have permission to claim car", 202);
        var car = DataStorage.Instance.CarDetails.FirstOrDefault(x => x.CarDataId == claimRequest.CarId);
        if (car is null) return new Response("Selected car not exists", 203);
        if (car.History.IsClaimed()) return new Response("Cannot claim car, because it is already claimed", 204);
        car.History.Claim(user.User);
        return new Response("", 0);
    }

    [HttpPost]
    public Response UpdateCar([FromHeader(Name="Token")] string usertoken, [FromBody] TelemetryData data)
    {
        var user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new Response("User are not logged in", 200);
        if (!user.User.Permission.UpdateCar)
            return new Response("User does not have permission to update car", 204);
        var car = DataStorage.Instance.CarDetails.FirstOrDefault(x => x.History.IsClaimedBy(user.User));
        if (car is null) return new Response("User cannot update car that not claimed by him", 205);
        car.History.Update(data);
        return new Response("", 0);
    }

    [HttpGet]
    public Response ReleaseCar([FromHeader(Name="Token")] string usertoken)
    {
        var user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new Response("User are not logged in", 200);
        if (!user.User.Permission.ReleaseCar)
            return new Response("User does not have permission to release car", 205);
        var car = DataStorage.Instance.CarDetails.FirstOrDefault(x => x.History.IsClaimedBy(user.User));
        if (car is null) return new Response("User cannot release car that not claimed by him", 206);
        car.History.Release();
        return new Response("", 0);
    }
}