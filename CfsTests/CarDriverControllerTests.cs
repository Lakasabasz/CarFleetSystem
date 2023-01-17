using System.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using CarFleetSystemServer.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CfsTests;

[TestFixture]
public class CarDriverControllerTests
{
    private HttpClient _client;

    public CarDriverControllerTests()
    {
        _client = new HttpClient();
    }
    
    private async Task _setupRoot()
    {
        Credentials cred = new(){Username = "root", Password = "admin"};
        StringContent sc = new StringContent(JsonConvert.SerializeObject(cred),
            MediaTypeHeaderValue.Parse("application/json"));
        var response = await _client.PostAsync("https://localhost:7080/api/user/Login", sc);
        UserLoginResponse? uresp =
            JsonConvert.DeserializeObject<UserLoginResponse>(await response.Content.ReadAsStringAsync());
        if (uresp is null || uresp.ErrorCode != 0)
        {
            Assert.Fail($"Root login error: {uresp?.ErrorDescription ?? "null"}");
            return;
        }

        if (_client.DefaultRequestHeaders.Contains("Token")) _client.DefaultRequestHeaders.Remove("Token");
        _client.DefaultRequestHeaders.Add("Token", uresp.UserToken);
    }

    [SetUp]
    public async Task Setup()
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
        await _setupRoot();
    }

    [TearDown]
    public void Shutdown()
    {
        
    }
    
    private async Task<int> _addTemplateCar(CarData cd)
    {
        var sc = new StringContent(JsonConvert.SerializeObject(cd), MediaTypeHeaderValue.Parse("application/json"));
        var resp = await _client.PutAsync("https://localhost:7080/api/fleet/AddCar", sc);
        AddCarResponse? response = JsonConvert.DeserializeObject<AddCarResponse>(await resp.Content.ReadAsStringAsync());
        Assert.That(resp.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.IsNotNull(response);
        Assert.That(response?.ErrorCode, Is.EqualTo(0));
        return response!.CreatedId;
    }
    
    [Test]
    public async Task AvailableCarListTest()
    {
        await _addTemplateCar(new CarData() { Mark = "Kek", Model = "Kekw", Plates = "XD", Vin = "XDDDD" });
        var resp = await _client.GetAsync("https://localhost:7080/api/car/List");
        AvailableCarsResponse? response = JsonConvert.DeserializeObject<AvailableCarsResponse>(await resp.Content.ReadAsStringAsync());
        Assert.IsNotNull(response);
        Assert.That(response?.ErrorCode, Is.EqualTo(0));
        Assert.That(response?.Cars.ToArray().Length, Is.GreaterThan(0));
    }

    private async Task<int> _claim()
    {
        int id = await _addTemplateCar(new CarData() { Mark = "Kek", Model = "Kekw", Plates = "XD", Vin = "XDDDD" });
        StringContent sc = new StringContent(JsonConvert.SerializeObject(new CarClaimRequest() { CarId = id }),
            MediaTypeHeaderValue.Parse("application/json"));
        await _client.PostAsync("https://localhost:7080/api/car/ClaimCar", sc);
        return id;
    }
    
    [Test]
    public async Task ClaimCarTest()
    {
        int id = await _addTemplateCar(new CarData() { Mark = "Kek", Model = "Kekw", Plates = "XD", Vin = "XDDDD" });
        StringContent sc = new StringContent(JsonConvert.SerializeObject(new CarClaimRequest() { CarId = id }),
            MediaTypeHeaderValue.Parse("application/json"));
        var resp = await _client.PostAsync("https://localhost:7080/api/car/ClaimCar", sc);
        Response? response = JsonConvert.DeserializeObject<Response>(await resp.Content.ReadAsStringAsync());
        Assert.IsNotNull(response);
        Assert.That(response?.ErrorCode, Is.EqualTo(0));
    }

    [Test]
    public async Task UpdateCarTest()
    {
        await _claim();
        StringContent sc = new StringContent(JsonConvert.SerializeObject(new TelemetryData()
            {Date = DateTime.Now, H = 21.37, W = 69.69}),
            MediaTypeHeaderValue.Parse("application/json"));
        var resp = await _client.PostAsync("https://localhost:7080/api/car/UpdateCar", sc);
        Response? response = JsonConvert.DeserializeObject<Response>(await resp.Content.ReadAsStringAsync());
        Assert.IsNotNull(response);
        Assert.That(response?.ErrorCode, Is.EqualTo(0));
    }

    [Test]
    public async Task ReleaseCarTest()
    {
        await _claim();
        var resp = await _client.GetAsync("https://localhost:7080/api/car/ReleaseCar");
        Response? response = JsonConvert.DeserializeObject<Response>(await resp.Content.ReadAsStringAsync());
        Assert.IsNotNull(response);
        Assert.That(response?.ErrorCode, Is.EqualTo(0));
    }
}