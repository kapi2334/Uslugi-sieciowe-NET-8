using WeatherAPI.Objects;
namespace WeatherAPI.Services
{
    public class CitiesDataService
    {
        public List<City> Cities { get; } = new()
        {
            new City {Id = 0, Name = "Warsaw", Country = "PL" },
            new City {Id = 1, Name = "Krakow", Country = "PL" },
            new City {Id = 2, Name = "Gdansk", Country = "PL" },
            new City {Id = 3, Name = "Bydgoszcz", Country = "PL" }
        };
    }
}
