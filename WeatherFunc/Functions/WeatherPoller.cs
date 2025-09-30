using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using WeatherFunc.Services;

namespace WeatherFunc.Functions;

public class WeatherPoller
{
    private readonly IWeatherService _service;
    private readonly ILogger _logger;

    public WeatherPoller(IWeatherService service, ILoggerFactory loggerFactory)
    {
        _service = service;
        _logger = loggerFactory.CreateLogger<WeatherPoller>();
    }

    [Function("WeatherPoller")]
    public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo timer)
    {
        {
            _logger.LogInformation($"Polling weather at: {DateTime.UtcNow}");
            await _service.PollAndStoreAsync();
        }
    }
}