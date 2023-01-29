using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CarFleetManager.models;
using Newtonsoft.Json;

namespace CarFleetManager;

public class ApiClient
{
    private static string? _userToken;
    private static string? _loggedInUsername;

    public static string? LoggedInUsername => _loggedInUsername;
    private static HttpClient _client = new();

    public static bool IsLoggedIn() => _userToken != null;
    
    public static async Task<bool> Login(string login, string password)
    {
        HttpResponseMessage resp;
        try
        {
            resp = await _client.PostAsync("https://localhost:7080/api/user/Login",
                new StringContent(JsonConvert.SerializeObject(new Dictionary<string, string>()
                {
                    {"Username", login},
                    {"Password", password}
                }), Encoding.Default, "application/json"));
        }
        catch (HttpRequestException)
        {
            return false;
        }

        var definition = new { ErrorCode = 0, ErrorDescription = "", UserToken = "" };
        var parsed = JsonConvert.DeserializeAnonymousType(await resp.Content.ReadAsStringAsync(), definition);
        if (parsed is null) return false;
        if (parsed.ErrorCode != 0)
        {
            Console.Error.WriteLine($"Login error, code: {parsed.ErrorCode}; description: {parsed.ErrorDescription} ");
            return false;
        }
        if (parsed.GetType().GetProperty("UserToken") is null) return false;
        _userToken = parsed.UserToken;
        _loggedInUsername = login;
        _client.DefaultRequestHeaders.Add("Token", _userToken);
        return true;
    }

    public static async Task<(bool, List<UserData>)> ListUsers()
    {
        var resp = await _client.GetAsync("https://localhost:7080/api/user/GetUsers");
        var definition = new { ErrorDescription = "", ErrorCode = 0, Users = new List<UserData>() };
        var parsed = JsonConvert.DeserializeAnonymousType(await resp.Content.ReadAsStringAsync(), definition);
        if (parsed is null) return (false, new List<UserData>());
        if (parsed.ErrorCode != 0)
        {
            Console.Error.WriteLine($"Users list fetch error: {parsed.ErrorCode}; description: {parsed.ErrorDescription}");
            return (false, new List<UserData>());
        }

        return (true, parsed.Users);
    }

    public static async Task<bool> EditUser(string? originalUsername, UserData currentUser)
    {
        var resp = await _client.PostAsync("https://localhost:7080/api/user/UpdateUser",
            new StringContent(JsonConvert.SerializeObject(new
            {
                OriginalUsername = originalUsername,
                Data = currentUser
            }), Encoding.Default, "application/json"));
        var definition = new { ErrorDescription = "", ErrorCode = 0 };
        var parsed = JsonConvert.DeserializeAnonymousType(await resp.Content.ReadAsStringAsync(), definition);
        if (parsed is null) return false;
        if (parsed.ErrorCode != 0)
        {
            Console.Error.WriteLine($"User edit error: {parsed.ErrorCode}; description: {parsed.ErrorDescription}");
            return false;
        }

        return true;
    }

    public static async Task<bool> AddUser(UserData currentUser)
    {
        var resp = await _client.PutAsync("https://localhost:7080/api/user/AddUser",
            new StringContent(JsonConvert.SerializeObject(new
            {
                currentUser.Username,
                currentUser.Password,
                currentUser.Permission
            }), Encoding.Default, "application/json"));
        var definition = new { ErrorDescription = "", ErrorCode = 0 };
        var parsed = JsonConvert.DeserializeAnonymousType(await resp.Content.ReadAsStringAsync(), definition);
        if (parsed is null) return false;
        if (parsed.ErrorCode != 0)
        {
            Console.Error.WriteLine($"User add error: {parsed.ErrorCode}; description: {parsed.ErrorDescription}");
            return false;
        }

        return true;
    }

    public static async Task<bool> DeleteUser(UserData currentUser)
    {
        var resp = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Delete, "https://localhost:7080/api/user/DeleteUser")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    currentUser.Username
                }), Encoding.Default, "application/json")
            });
        var definition = new { ErrorDescription = "", ErrorCode = 0 };
        var parsed = JsonConvert.DeserializeAnonymousType(await resp.Content.ReadAsStringAsync(), definition);
        if (parsed is null) return false;
        if (parsed.ErrorCode != 0)
        {
            Console.Error.WriteLine($"User deletion error: {parsed.ErrorCode}; description: {parsed.ErrorDescription}");
            return false;
        }

        return true;
    }

    public static async Task<(bool, List<CarData>)> ListCars()
    {
        var resp = await _client.GetAsync("https://localhost:7080/api/fleet/List");
        var definition = new { ErrorDescription = "", ErrorCode = 0, Cars = new List<CarData>() };
        var parsed = JsonConvert.DeserializeAnonymousType(await resp.Content.ReadAsStringAsync(), definition);
        if (parsed is null) return (false, new List<CarData>());
        if (parsed.ErrorCode != 0)
        {
            Console.Error.WriteLine($"Cars list fetch error: {parsed.ErrorCode}; description: {parsed.ErrorDescription}");
            return (false, new List<CarData>());
        }

        return (true, parsed.Cars);
    }

    public static async Task<bool> AddCar(CarData currentCar)
    {
        var resp = await _client.PutAsync("https://localhost:7080/api/fleet/AddCar",
            new StringContent(JsonConvert.SerializeObject(currentCar), Encoding.Default, "application/json"));
        var definition = new { ErrorDescription = "", ErrorCode = 0 };
        var parsed = JsonConvert.DeserializeAnonymousType(await resp.Content.ReadAsStringAsync(), definition);
        if (parsed is null) return false;
        if (parsed.ErrorCode != 0)
        {
            Console.Error.WriteLine($"Cars add error: {parsed.ErrorCode}; description: {parsed.ErrorDescription}");
            return false;
        }

        return true;
    }

    public static async Task<bool> EditCar(int originalCarId, CarData currentCar)
    {
        var resp = await _client.PostAsync("https://localhost:7080/api/fleet/UpdateCar",
            new StringContent(JsonConvert.SerializeObject(new
            {
                CarId = originalCarId,
                Data = currentCar
            }), Encoding.Default, "application/json"));
        var definition = new { ErrorDescription = "", ErrorCode = 0 };
        var parsed = JsonConvert.DeserializeAnonymousType(await resp.Content.ReadAsStringAsync(), definition);
        if (parsed is null) return false;
        if (parsed.ErrorCode != 0)
        {
            Console.Error.WriteLine($"Car edit error: {parsed.ErrorCode}; description: {parsed.ErrorDescription}");
            return false;
        }

        return true;
    }
    
    public static async Task<bool> DeleteCar(CarData currentCar)
    {
        var resp = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Delete, "https://localhost:7080/api/fleet/DeleteCar")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { currentCar.Id }), Encoding.Default,
                    "application/json")
            });
        var definition = new { ErrorDescription = "", ErrorCode = 0 };
        var parsed = JsonConvert.DeserializeAnonymousType(await resp.Content.ReadAsStringAsync(), definition);
        if (parsed is null) return false;
        if (parsed.ErrorCode != 0)
        {
            Console.Error.WriteLine($"Car delete error: {parsed.ErrorCode}; description: {parsed.ErrorDescription}");
            return false;
        }

        return true;
    }

    public static async Task<bool> EditCarDetails(int carId, CarDetailsData currentCarDetails)
    {
        if(currentCarDetails.CarColor is null) currentCarDetails.CarColor = Color.Convert(System.Drawing.Color.Black);
        var resp = await _client.PostAsync("https://localhost:7080/api/fleet/UpdateCarDetails",
            new StringContent(JsonConvert.SerializeObject(new
            {
                CarId = carId,
                Data = currentCarDetails
            }), Encoding.Default, "application/json"));
        var definition = new { ErrorDescription = "", ErrorCode = 0 };
        var parsed = JsonConvert.DeserializeAnonymousType(await resp.Content.ReadAsStringAsync(), definition);
        if (parsed is null) return false;
        if (parsed.ErrorCode != 0)
        {
            Console.Error.WriteLine($"Car details update error: {parsed.ErrorCode}; description: {parsed.ErrorDescription}");
            return false;
        }

        return true;
    }

    public static async Task<(bool result, CarDetailsData report)> GetCarDetails(int carId)
    {
        var resp = await _client.SendAsync(
            new HttpRequestMessage(HttpMethod.Get, "https://localhost:7080/api/fleet/GetCarDetails")
        {
            Content = new StringContent(JsonConvert.SerializeObject(new { Id = carId }), Encoding.Default,
                "application/json")
        });
        
        var definition = new { ErrorDescription = "", ErrorCode = 0, Report = new CarReport() };
        var parsed = JsonConvert.DeserializeAnonymousType(await resp.Content.ReadAsStringAsync(), definition);
        if (parsed is null) return (false, new CarDetailsData());
        if (parsed.ErrorCode != 0)
        {
            Console.Error.WriteLine($"Car details update error: {parsed.ErrorCode}; description: {parsed.ErrorDescription}");
            return (false, new CarDetailsData());
        }

        return (true, parsed.Report.Details);
    }

}