using Newtonsoft.Json;

namespace WeatherWorkerService;

public class Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cities = new[] { "Warszawa", "Chełm", "Lublin" };
        var httpClient = new HttpClient();
        var apiKey = "Twój klucz API";

        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var city in cities)
            {
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&lang=pl&appid={apiKey}";

                try
                {
                    var response = await httpClient.GetAsync(url, stoppingToken);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(content);

                    using (var scope = scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        SavedWeatherData dataToBeSaved = new SavedWeatherData();
                        dataToBeSaved.Map(weatherData);

                        dbContext.WeatherData.Add(dataToBeSaved);
                        await dbContext.SaveChangesAsync(stoppingToken);
                    } // Tutaj dbContext bezpiecznie się zamyka i czyści pamięć          
                }
                catch (HttpRequestException ex)
                {
                    logger.LogError($"Błąd przy próbie pobrania pogody dla miasta {city}: {ex.Message}");
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
