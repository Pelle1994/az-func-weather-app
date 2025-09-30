using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherFunc.Data;
using WeatherFunc.Services;
using Microsoft.EntityFrameworkCore;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var weatherApiUrl = builder.Configuration["WeatherApiUrl"];

// Service with Weather HttpClient
builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
{
    if (string.IsNullOrWhiteSpace(weatherApiUrl))
    {
        throw new InvalidOperationException("WeatherApiUrl configuration is missing.");
    }
    client.BaseAddress = new Uri(weatherApiUrl);
});

// DB
builder.Services.AddDbContext<WeatherDbContext>(options =>
            options.UseSqlServer(builder.Configuration["SqlConnectionString"]));

// Azure stuff
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
