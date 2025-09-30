using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherFunc.Models;


namespace WeatherFunc.Services
{
    public interface IWeatherService
    {
        Task PollAndStoreAsync();

        Task<WeatherData> GetWeatherAsync(DateTime request);
    }
}
