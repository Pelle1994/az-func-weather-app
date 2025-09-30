using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using WeatherFunc.Services;

namespace WeatherFunc.Functions;

public class GetTemperatures
{
    private readonly IWeatherService _service;
    public GetTemperatures(IWeatherService service) => _service = service;


    [Function("GetTemperature")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "temperature")] HttpRequestData req)
    {

        var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);

        if (!DateTime.TryParse(query.Get("timestamp"), out var timestamp))
        {
            var bad = req.CreateResponse(HttpStatusCode.BadRequest);

            var example = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            await bad.WriteStringAsync(
                $"Invalid or missing timestamp query param. Expected format: \"YYYY-MM-DDTHH:MM:SSZ\" (UTC). Example: ?timestamp={example}"
            );
            return bad;
        }


        try
        {
            var record = await _service.GetWeatherAsync(timestamp);

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(record);

            return response;
        }
        catch (Exception ex) 
        { 
            var notfound = req.CreateResponse(HttpStatusCode.NotFound);

            await notfound.WriteAsJsonAsync(ex.Message);

            return notfound;
        }
        
    }
}
