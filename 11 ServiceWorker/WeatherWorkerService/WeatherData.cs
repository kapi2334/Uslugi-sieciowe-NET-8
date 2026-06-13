using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WeatherWorkerService
{
    public class WeatherData
    {
        [JsonPropertyName("list")]
        public List<ForecastItem> Forecasts { get; set; }
    }
    public class ForecastItem
    {
        [JsonPropertyName("dt")]
        public long UnixDateTime { get; set; }
        public Main Main { get; set; }
        public List<Weather> Weather { get; set; }
        public Wind Wind { get; set; }
        public int Visibility { get; set; }

        public Rain Rain { get; set; }
        public Snow Snow { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; }
    }


    public class Wind
    {
        public double Speed { get; set; }
    }

    public class Rain
    {
        [JsonPropertyName("3h")]
        public double? Volume3h { get; set; }
    }

    public class Snow
    {
        [JsonPropertyName("3h")]
        public double? Volume3h { get; set; }
    }
}
