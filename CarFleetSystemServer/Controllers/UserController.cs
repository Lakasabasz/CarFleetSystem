using CarFleetSystemServer.Models;
using Microsoft.AspNetCore.Mvc;
using CarFleetSystemServer.Tools;
using static BCrypt.Net.BCrypt;

namespace CarFleetSystemServer.Controllers;

[ApiController]
public class UserController : Controller
{
    [HttpPost]
    public UserLoginResponse Login([FromBody] string username, [FromBody] string password)
    {
        var user = DataStorage.Instance.Users.FirstOrDefault(x => x.Username == username);
        if (user == null || !EnhancedVerify(user.Password, EnhancedHashPassword(password)))
            return new UserLoginResponse("User login and/or password are invalid", 1);
        var loggedIn = new LoggedInUser(user);
        DataStorage.Instance.LoggedInUsers.Add(loggedIn);
        DataStorage.Instance.SaveChanges();
        return new UserLoginResponse()
        {
            UserToken = loggedIn.UserToken
        };
    }

    [HttpGet]
    public Response Logout([FromBody] string usertoken)
    {
        LoggedInUser? user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new Response("User is not logged in", 2);
        DataStorage.Instance.LoggedInUsers.Remove(user);
        DataStorage.Instance.SaveChanges();
        return new Response("", 0);
    }

    [HttpGet]
    public JsonResult Prolong(string usertoken)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public JsonResult GetUsers(string usertoken)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    public JsonResult AddUser(string usertoken, UserData data)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public JsonResult UpdateUser(string usertoken, string editedUsertoken, UserData data)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public JsonResult DeleteUser(string usertoken, string editedUsertoken)
    {
        throw new NotImplementedException();
    }
}