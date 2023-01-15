using System.Net.Http.Headers;
using System.Net.Sockets;
using CarFleetSystemServer.Models;
using Newtonsoft.Json;

namespace CfsTests;

public class UserControllerTests
{
    private async Task<string?> _rootLogin(HttpClient client)
    {
        Credentials cred = new(){Username = "root", Password = "admin"};
        StringContent sc = new StringContent(JsonConvert.SerializeObject(cred),
            MediaTypeHeaderValue.Parse("application/json"));
        var response = await client.PostAsync("https://localhost:7080/api/user/Login", sc);
        UserLoginResponse? uresp =
            JsonConvert.DeserializeObject<UserLoginResponse>(await response.Content.ReadAsStringAsync());
        return uresp?.UserToken;
    }
    
    [SetUp]
    public void Setup()
    {
        TcpClient client = new TcpClient();
        try
        {
            client.Connect("localhost", 7080);
        }
        catch (Exception e)
        {
            Assert.Fail("Service not enabled");
        }
        client.Close();
    }

    [TearDown]
    public void Shutdown()
    {
        
    }

    [Test]
    public async Task Connection()
    {
        HttpClient client = new HttpClient();
        var data = await client.GetStringAsync("https://localhost:7080/");
        Assert.Greater(data.Length, 0);
    }

    [Test]
    public async Task Login()
    {
        HttpClient client = new();
        Credentials cred = new(){Username = "root", Password = "admin"};
        StringContent sc = new StringContent(JsonConvert.SerializeObject(cred),
            MediaTypeHeaderValue.Parse("application/json"));
        var response = await client.PostAsync("https://localhost:7080/api/user/Login", sc);
        UserLoginResponse? uresp =
            JsonConvert.DeserializeObject<UserLoginResponse>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(uresp);
        Assert.IsNotEmpty(uresp?.UserToken);
        Assert.That(uresp?.ErrorCode, Is.EqualTo(0), uresp?.ErrorDescription);
    }
    
    [Test]
    public async Task Logout()
    {
        HttpClient client = new();
        string? token = await _rootLogin(client);
        client.DefaultRequestHeaders.Add("Token", token);
        var response = await client.GetAsync("https://localhost:7080/api/user/Logout");
        Response? resp = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(resp);
        Assert.That(resp?.ErrorCode, Is.EqualTo(0));
    }

    [Test]
    public async Task GetUser()
    {
        HttpClient client = new();
        string? token = await _rootLogin(client);
        client.DefaultRequestHeaders.Add("Token", token);
        var response = await client.GetAsync("https://localhost:7080/api/user/GetUsers");
        UserDataListResponse? resp = JsonConvert.DeserializeObject<UserDataListResponse>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(resp);
        Assert.That(resp?.ErrorCode, Is.EqualTo(0));
        Assert.NotNull(resp?.Users);
        Assert.That(resp?.Users.ToList().Count, Is.GreaterThan(0));
    }
    
    [Test]
    public async Task AddUser()
    {
        HttpClient client = new();
        string? token = await _rootLogin(client);
        client.DefaultRequestHeaders.Add("Token", token);
        UserCreateRequest ucr = new()
        {
            Username = $"New profile {DateTime.Now.Ticks}",
            Password = "Password to hash",
            Permission = new()
            {
                ViewUserList = true
            }
        };
        var sc = new StringContent(JsonConvert.SerializeObject(ucr), MediaTypeHeaderValue.Parse("application/json"));
        var response = await client.PutAsync("https://localhost:7080/api/user/AddUser", sc);
        Response? resp = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(resp);
        Assert.That(resp?.ErrorCode, Is.EqualTo(0));
    }

    [Test]
    public async Task EditUser()
    {
        HttpClient client = new();
        string? token = await _rootLogin(client);
        client.DefaultRequestHeaders.Add("Token", token);
        UserCreateRequest urc = new()
        {
            Username = $"Test edit {DateTime.Now.Ticks}", Password = "pass", Permission = new() { ViewUserList = true }
        };
        var sc = new StringContent(JsonConvert.SerializeObject(urc), MediaTypeHeaderValue.Parse("application/json"));
        await client.PutAsync("https://localhost:7080/api/user/AddUser", sc);
        UserEditRequest uer = new()
        {
            OriginalUsername = urc.Username,
            Data = new UserData()
            {
                Username = urc.Username,
                Password = "newPass",
                Permission = new() { ViewUserList = true }
            }
        };
        sc = new StringContent(JsonConvert.SerializeObject(uer),
            MediaTypeWithQualityHeaderValue.Parse("application/json"));
        var response = await client.PostAsync("https://localhost:7080/api/user/UpdateUser", sc);
        Response? resp = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(resp);
        Assert.That(resp?.ErrorCode, Is.EqualTo(0), resp?.ErrorDescription);
    }
    
    [Test]
    public async Task DeleteUser()
    {
        HttpClient client = new();
        string? token = await _rootLogin(client);
        client.DefaultRequestHeaders.Add("Token", token);
        UserCreateRequest urc = new()
        {
            Username = $"Test delete {DateTime.Now.Ticks}", Password = "pass", Permission = new() { ViewUserList = true }
        };
        var sc = new StringContent(JsonConvert.SerializeObject(urc), MediaTypeHeaderValue.Parse("application/json"));
        await client.PutAsync("https://localhost:7080/api/user/AddUser", sc);
        UserDeleteRequest uer = new()
        {
            Username = urc.Username
        };
        sc = new StringContent(JsonConvert.SerializeObject(uer),
            MediaTypeWithQualityHeaderValue.Parse("application/json"));
        var response = await client.SendAsync(
            new HttpRequestMessage(HttpMethod.Delete, "https://localhost:7080/api/user/DeleteUser")
            {
                Content = sc
            });
        Response? resp = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(resp);
        Assert.That(resp?.ErrorCode, Is.EqualTo(0));
    }
}