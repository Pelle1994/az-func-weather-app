using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherFunc.Data;
using WeatherFunc.Models;

namespace WeatherFunc.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _http;
        private readonly WeatherDbContext _db;


        public WeatherService(HttpClient http, WeatherDbContext db)
        {
            _http = http;
            _db = db;
        }

        public async Task PollAndStoreAsync()
        {
            var json = await _http.GetStringAsync("");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<OpenMeteoResponse>(json, options);

            if (data != null)
            {
                for (int i = 0; i < data.hourly.time.Count; i++)
                {
                    if (data.hourly.temperature_2m[i] is null) continue; // Skip nulls

                    var timestamp = DateTime.Parse(data.hourly.time[i]).ToUniversalTime();
                    var temp = data.hourly.temperature_2m[i].Value;

                    var entity = await _db.WeatherData.FindAsync(timestamp);

                    if (entity == null)
                    {
                        _db.WeatherData.Add(new WeatherData { TimestampUtc = timestamp, TemperatureC = temp });
                    }
                    else
                    {
                        entity.TemperatureC = temp; // Upsert
                    }
                }

                await _db.SaveChangesAsync();
            }
            
        }

        public async Task<WeatherData> GetWeatherAsync(DateTime request)
        {
            var requestUtc = request.ToUniversalTime();
            var entity = await _db.WeatherData.FindAsync(requestUtc);

            if (entity != null)
                return entity;

            throw new Exception($"No temperatures for date: {requestUtc} was found");
           
        }
    }
}
