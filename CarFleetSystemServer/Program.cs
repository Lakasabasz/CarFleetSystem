using CarFleetSystemServer.Models;
using CarFleetSystemServer.Tools;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

public class ApplicationPartsLogger : IHostedService
{
    private readonly ILogger<ApplicationPartsLogger> _logger;
    private readonly ApplicationPartManager _partManager;

    public ApplicationPartsLogger(ILogger<ApplicationPartsLogger> logger, ApplicationPartManager partManager)
    {
        _logger = logger;
        _partManager = partManager;
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        var applicationParts = _partManager.ApplicationParts.Select(x => x.Name);

        var controllerFeature = new ControllerFeature();
        _partManager.PopulateFeature(controllerFeature);
        var controllers = controllerFeature.Controllers.Select(x => x.Name);

        _logger.LogInformation("Found the following application parts: '{ApplicationParts}', with the following controllers: '{Controllers}'",
            string.Join(", ", applicationParts), string.Join(", ", controllers));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

public class Server
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddHostedService<ApplicationPartsLogger>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        DataStorage.Init(new RamDataStorage()
        {
            Users = { new UserData()
            {
                Username = "root",
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword("admin"),
                Permission = new PermissionSet()
                {
                    Root = true
                }
            } }
        });

        app.Run();
    }
}