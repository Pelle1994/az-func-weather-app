using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherFunc.Models
{

    public class OpenMeteoResponse
    {
        public Hourly hourly { get; set; }
    }

    public class Hourly
    {
        public List<string> time { get; set; }
        public List<double?> temperature_2m { get; set; }
    }


}
