using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherFunc.Models
{
    public class WeatherData
    {
        public DateTime TimestampUtc { get; set; }  // PK
        public double TemperatureC { get; set; }
    }
}
