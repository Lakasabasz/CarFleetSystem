using CarFleetSystemServer.Models;
using CarFleetSystemServer.Tools;
using Microsoft.AspNetCore.Mvc;

namespace CarFleetSystemServer.Controllers;

[ApiController, Route("api/fleet/[action]")]
public class CarManagementController : Controller
{
    [HttpGet]
    public CarListResponse List([FromHeader(Name = "Token")] string usertoken) // List all cars
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
    public AddCarResponse AddCar([FromHeader(Name = "Token")] string usertoken, [FromBody] CarData data)
    {
        var user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new AddCarResponse("User are not logged in", 100);
        if (!user.User.Permission.AddCar)
            return new AddCarResponse("User does not have permission to add new car", 102);
        if (data.Id.HasValue && DataStorage.Instance.Cars.FirstOrDefault(x => x.Id == data.Id) is not null)
            return new AddCarResponse("Car already exists", 103);
        data.Id ??= (DataStorage.Instance.Cars.Max(x => x.Id) ?? 0) + 1;
        DataStorage.Instance.Cars.Add(data);
        DataStorage.Instance.CarDetails.Add(new CarDetails(data));
        return new AddCarResponse() { CreatedId = data.Id.Value };
    }

    [HttpPost]
    public Response UpdateCar([FromHeader(Name = "Token")] string usertoken, [FromBody] CarUpdateRequest updateRequest)
    {
        var user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new Response("User are not logged in", 100);
        if (!user.User.Permission.EditCar)
            return new Response("User does not have permission to edit car", 104);
        CarData? current = DataStorage.Instance.Cars.FirstOrDefault(x => x.Id == updateRequest.CarId);
        if (current is null) return new Response("Car does not exists", 105);
        if (updateRequest.Data.Id.HasValue
            && current.Id != updateRequest.Data.Id
            && DataStorage.Instance.Cars.FirstOrDefault(x => x.Id == updateRequest.Data.Id) is not null)
            return new Response("Car new id is already taken by other one", 106);
        if (current.Id != updateRequest.Data.Id && updateRequest.Data.Id.HasValue)
        {
            CarDetails details = DataStorage.Instance.CarDetails.First(x => x.CarDataId == current.Id);
            details.CarDataId = updateRequest.Data.Id.Value;
        }
        if(updateRequest.Data.Id.HasValue) current.Id = updateRequest.Data.Id;
        current.Mark = updateRequest.Data.Mark;
        current.Model = updateRequest.Data.Model;
        current.Plates = updateRequest.Data.Plates;
        current.Vin = updateRequest.Data.Vin;
        return new Response("", 0);
    }

    [HttpDelete]
    public Response DeleteCar([FromHeader(Name = "Token")]string usertoken, [FromBody]CarDeleteRequest deleteRequest)
    {
        var user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new Response("User are not logged in", 100);
        if (!user.User.Permission.DeleteCar)
            return new Response("User does not have permission to delete car", 107);
        var car = DataStorage.Instance.Cars.FirstOrDefault(x => x.Id == deleteRequest.Id);
        var details = DataStorage.Instance.CarDetails.FirstOrDefault(x => x.CarDataId == deleteRequest.Id);
        if(car is null || details is null) return new Response("Car does not exists", 105);
        DataStorage.Instance.Cars.Remove(car);
        DataStorage.Instance.CarDetails.Remove(details);
        return new Response("", 0);
    }

    [HttpGet]
    public CarDetailsResponse GetCarDetails([FromHeader(Name = "Token")] string usertoken, [FromBody] CarDetailsRequest detailsRequest)
    {
        var user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new CarDetailsResponse("User are not logged in", 100);
        if (!user.User.Permission.ViewCarDetails)
            return new CarDetailsResponse("User does not have permission to view car details", 108);
        var details = DataStorage.Instance.CarDetails.FirstOrDefault(x => x.CarDataId == detailsRequest.Id);
        if(details is null) return new CarDetailsResponse("Car details does not exists", 109);
        return new CarDetailsResponse()
        {
            Report = new CarReport(){Details = details.Details, Summary = details.History.GetSummary()}
        };
    }

    [HttpPost]
    public Response UpdateCarDetails([FromHeader(Name = "Token")] string usertoken, CarDetailsUpdateRequest updateRequest)
    {
        var user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new CarDetailsResponse("User are not logged in", 100);
        if (!user.User.Permission.UpdateCarDetails)
            return new CarDetailsResponse("User does not have permission to edit car details", 110);
        var details = DataStorage.Instance.CarDetails.FirstOrDefault(x => x.CarDataId == updateRequest.CarId);
        if(details is null) return new CarDetailsResponse("Car details does not exists", 109);
        if (details.CarDataId != updateRequest.Data.CarDataId)
            return new Response("User cannot change details id using this endpoint", 111);
        details.Details.Description = updateRequest.Data.Details.Description;
        details.Details.CarColor = updateRequest.Data.Details.CarColor;
        details.Details.InsuranceEnding = updateRequest.Data.Details.InsuranceEnding;
        details.Details.NextCarReview = updateRequest.Data.Details.NextCarReview;
        return new Response("", 0);
    }
}