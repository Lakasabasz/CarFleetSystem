using CarFleetSystemServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarFleetSystemServer.Controllers;

[ApiController]
public class UserController : Controller
{
    [HttpPost]
    public JsonResult Login(string username, string password)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public JsonResult Logout(string usertoken)
    {
        throw new NotImplementedException();
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