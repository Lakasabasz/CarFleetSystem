using System.Drawing;
using System.Net.Sockets;
using CarFleetSystemServer.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Color = CarFleetSystemServer.Models.Color;

namespace CfsTests;

[TestFixture]
public class CarFleetManagerTests
{
    private HttpClient _client;

    public CarFleetManagerTests()
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

    [Test]
    public async Task AddCarTest()
    {
        var sc = new StringContent(JsonConvert.SerializeObject(new CarData()
        {
            Mark = "A",
            Model = "B",
            Plates = "SB 2137X",
            Vin = "JTEGD20V550092056"
        }), MediaTypeHeaderValue.Parse("application/json"));
        var resp = await _client.PutAsync("https://localhost:7080/api/fleet/AddCar", sc);
        AddCarResponse? response = JsonConvert.DeserializeObject<AddCarResponse>(await resp.Content.ReadAsStringAsync());
        Assert.IsNotNull(response);
        Assert.That(response?.ErrorCode, Is.EqualTo(0));
        Assert.That(response?.CreatedId, Is.GreaterThan(0));
    }

    private async Task<int> _addTemplateCar(CarData cd)
    {
        var sc = new StringContent(JsonConvert.SerializeObject(cd), MediaTypeHeaderValue.Parse("application/json"));
        var resp = await _client.PutAsync("https://localhost:7080/api/fleet/AddCar", sc);
        AddCarResponse? response = JsonConvert.DeserializeObject<AddCarResponse>(await resp.Content.ReadAsStringAsync());
        Assert.IsNotNull(response);
        Assert.That(response?.ErrorCode, Is.EqualTo(0));
        return response!.CreatedId;
    }
    
    [Test]
    public async Task ListCarTest()
    {
        await _addTemplateCar(new CarData() { Mark = "A", Model = "B", Plates = $"AD {new Random().Next(99999)}", Vin = "D" });
        var resp = await _client.GetAsync("https://localhost:7080/api/fleet/List");
        CarListResponse? response = JsonConvert.DeserializeObject<CarListResponse>(await resp.Content.ReadAsStringAsync());
        Assert.IsNotNull(response);
        Assert.That(response?.ErrorCode, Is.EqualTo(0));
        Assert.That(response?.Cars.ToArray().Length, Is.GreaterThan(0));
    }
    
    [Test]
    public async Task EditCarTest()
    {
        CarData cd = new CarData()
            { Mark = "E", Model = "C", Plates = $"ED {new Random().Next(99999)}", Vin = "D" };
        int id = await _addTemplateCar(cd);
        cd.Vin = $"D{new Random().NextInt64()}";
        CarUpdateRequest cur = new(){Data = cd, CarId = id};
        StringContent sc = new StringContent(JsonConvert.SerializeObject(cur),
            MediaTypeHeaderValue.Parse("application/json"));
        var resp = await _client.PostAsync("https://localhost:7080/api/fleet/UpdateCar", sc);
        Response? response = JsonConvert.DeserializeObject<Response>(await resp.Content.ReadAsStringAsync());
        Assert.IsNotNull(response);
        Assert.That(response?.ErrorCode, Is.EqualTo(0));
    }
    
    [Test]
    public async Task DeleteCarTest()
    {
        CarData cd = new CarData()
            { Mark = "E", Model = "C", Plates = $"DE {new Random().Next(99999)}", Vin = "D" };
        int id = await _addTemplateCar(cd);
        StringContent sc = new StringContent(JsonConvert.SerializeObject(new CarDeleteRequest() { Id = id }),
            MediaTypeHeaderValue.Parse("application/json"));
        var resp = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Delete, "https://localhost:7080/api/fleet/DeleteCar")
            {
                Content = sc
            });
        Response? response = JsonConvert.DeserializeObject<Response>(await resp.Content.ReadAsStringAsync());
        Assert.IsNotNull(response);
        Assert.That(response?.ErrorCode, Is.EqualTo(0));
    }
    
    [Test]
    public async Task GetDetailsCarTest()
    {
        CarData cd = new CarData()
            { Mark = "E", Model = "C", Plates = $"GD {new Random().Next(99999)}", Vin = "D" };
        int id = await _addTemplateCar(cd);
        StringContent sc = new StringContent(JsonConvert.SerializeObject(new CarDetailsRequest() { Id = id }),
            MediaTypeHeaderValue.Parse("application/json"));
        var resp = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, "https://localhost:7080/api/fleet/GetCarDetails")
            {
                Content = sc
            });
        CarDetailsResponse? response =
            JsonConvert.DeserializeObject<CarDetailsResponse>(await resp.Content.ReadAsStringAsync());
        Assert.IsNotNull(response);
        Assert.That(response?.ErrorCode, Is.EqualTo(0));
        Assert.IsNotNull(response?.Report.Details);
        Assert.IsNotNull(response?.Report.Summary);
    }
    
    [Test]
    public async Task UpdateDetailsCarTest()
    {
        CarData cd = new CarData()
            { Mark = "E", Model = "C", Plates = $"UD {new Random().Next(99999)}", Vin = "D" };
        int id = await _addTemplateCar(cd);
        StringContent sc = new StringContent(JsonConvert.SerializeObject(new CarDetailsUpdateRequest()
            {
                CarId = id, Data = new CarDetailsData()
                {
                    Description = "Description", CarColor = Color.Convert(System.Drawing.Color.Black), 
                    InsuranceEnding = DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
                    NextCarReview = DateOnly.FromDateTime(DateTime.Now.AddYears(1))
                }
            }),
            MediaTypeHeaderValue.Parse("application/json"));
        var resp = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Post, "https://localhost:7080/api/fleet/UpdateCarDetails")
            {
                Content = sc
            });
        Response? response = JsonConvert.DeserializeObject<Response>(await resp.Content.ReadAsStringAsync());
        Assert.IsNotNull(response);
        Assert.That(response?.ErrorCode, Is.EqualTo(0), response?.ErrorDescription);
    }

}