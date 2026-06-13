using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherWorkerService
{
    public class SavedWeatherData
    {
        public int Id { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }

        public void Map(WeatherData input)
        {
            Temperature = input.Forecasts[0].Main.Temp;
            Pressure = input.Forecasts[0].Main.Pressure;
            Humidity = input.Forecasts[0].Main.Humidity;
        }
    };
}
