using BCrypt.Net;
using CarFleetSystemServer.Models;
using Microsoft.AspNetCore.Mvc;
using CarFleetSystemServer.Tools;
using static BCrypt.Net.BCrypt;

namespace CarFleetSystemServer.Controllers;

[ApiController, Route("api/user/[action]")]
public class UserController : Controller
{
    private ILogger _log;
    public UserController(ILogger<UserController> logger)
    {
        _log = logger;
    }
    
    [HttpPost]
    public UserLoginResponse Login([FromBody] Credentials credentials)
    {
        var user = DataStorage.Instance.Users.FirstOrDefault(x => x.Username == credentials.Username);
        if (user == null || !EnhancedVerify(credentials.Password, user.Password))
        {
            _log.LogWarning("Connection from {ConnectionRemoteIpAddress}:{ConnectionRemotePort} failed to log in using credentials for {CredentialsUsername}. Reason: {User}",
                Request.HttpContext.Connection.RemoteIpAddress,
                Request.HttpContext.Connection.RemotePort, 
                credentials.Username,
                user is null ? "User not found" : "Wrong password");
            _log.LogDebug("Registered users: {Join}",
                String.Join(", ", DataStorage.Instance.Users.Select(x => x.Username)));
            return new UserLoginResponse("User login and/or password are invalid", 1);
        }
        var loggedIn = new LoggedInUser(user);
        DataStorage.Instance.LoggedInUsers.Add(loggedIn);
        DataStorage.Instance.SaveChanges();
        _log.LogInformation("Connection from {ConnectionRemoteIpAddress}:{ConnectionRemotePort} successfully logged in using credentials for {CredentialsUsername}",
            Request.HttpContext.Connection.RemoteIpAddress,
            Request.HttpContext.Connection.RemotePort,
            credentials.Username);
        return new UserLoginResponse()
        {
            UserToken = loggedIn.UserToken
        };
    }

    [HttpGet]
    public Response Logout([FromHeader(Name = "Token")] string usertoken)
    {
        LoggedInUser? user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new Response("User is not logged in", 2);
        DataStorage.Instance.LoggedInUsers.Remove(user);
        DataStorage.Instance.SaveChanges();
        return new Response("", 0);
    }

    [HttpGet]
    public Response Prolong([FromHeader(Name = "Token")] string usertoken)
    {
        LoggedInUser? user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new Response("User is not logged in", 2);
        user.Prolong();
        DataStorage.Instance.SaveChanges();
        return new Response("", 0);
    }

    [HttpGet]
    public UserDataListResponse GetUsers([FromHeader(Name = "Token")] string usertoken)
    {
        LoggedInUser? user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new UserDataListResponse("User is not logged in", 2);
        if (!user.User.Permission.ViewUserList)
            return new UserDataListResponse("User has not permission to view users list", 3);
        var userList = DataStorage.Instance.Users.ToList();
        return new UserDataListResponse()
        {
            Users = userList
        };
    }

    [HttpPut]
    public Response AddUser([FromHeader(Name = "Token")] string usertoken, [FromBody] UserCreateRequest data)
    {
        LoggedInUser? user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new Response("User is not logged in", 2);
        if (!user.User.Permission.AddUser)
            return new Response("User has not permission to add user", 4);
        if (DataStorage.Instance.Users.FirstOrDefault(x => data.Username == x.Username) is not null)
            return new Response("User with that username already exists", 5);
        if (!user.User.Permission.SetPermissions)
            data.Permission = new PermissionSet()
            {
                Root = false
            };
        DataStorage.Instance.Users.Add(UserData.From(data));
        return new Response("", 0);
    }

    [HttpPost]
    public Response UpdateUser([FromHeader(Name = "Token")] string usertoken, [FromBody] UserEditRequest request)
    {
        string originalUsername = request.OriginalUsername;
        UserData data = request.Data;
        LoggedInUser? user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new Response("User is not logged in", 2);
        if (!user.User.Permission.EditUser)
            return new Response("User has not permission to edit user", 6);
        if (DataStorage.Instance.Users.FirstOrDefault(x => data.Username == x.Username) is not null
            && originalUsername != data.Username)
            return new Response("User with that username already exists", 5);
        UserData? current = DataStorage.Instance.Users.FirstOrDefault(x => x.Username == originalUsername);
        if (current is null)
            return new Response("User with that username not exists", 7);
        if (data.Permission != current.Permission && !user.User.Permission.SetPermissions)
            return new Response("User do not have permission to edit permissions", 8);
        if (!user.User.Permission.Root && (current.Permission.Root || data.Permission.Root))
            return new Response("User without root cannot edit root users or promote them", 9);
        try
        {
            InterrogateHash(data.Password);
        }
        catch (HashInformationException)
        {
            data.Password = EnhancedHashPassword(data.Password);
        }
        current.Permission = data.Permission;
        current.Password = data.Password;
        current.Username = data.Username;
        return new Response("", 0);
    }

    [HttpDelete]
    public Response DeleteUser([FromHeader(Name = "Token")] string usertoken, [FromBody] UserDeleteRequest data)
    {
        string username = data.Username;
        LoggedInUser? user = DataStorage.Instance.LoggedInUsers.FirstOrDefault(x => x.UserToken == usertoken);
        if (user is null) return new Response("User is not logged in", 2);
        if (!user.User.Permission.DeleteUser)
            return new Response("User has not permission to delete user", 10);
        UserData? current = DataStorage.Instance.Users.FirstOrDefault(x => x.Username == username);
        if (current is null)
            return new Response("User with that username not exists", 7);
        if (!user.User.Permission.Root && current.Permission.Root)
            return new Response("User without root cannot delete root users", 11);
        DataStorage.Instance.Users.Remove(current);
        return new Response("", 0);
    }
}