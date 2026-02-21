using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.Json;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
	"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weather", async Task<IResult> () =>
{
	//Lat & lon for Warsaw, 
	
	var handler = new HttpClientHandler();
	using var httpClient = new HttpClient(handler)
	{
		BaseAddress = new Uri("https://api.openweathermap.org/")
	};
	/*
		Paste your openwheathermap API key to the variable below.
	 */
	string apiKey = "YOUR_API_KEY_GOES_HERE";
	using HttpResponseMessage response = await httpClient.GetAsync($"data/2.5/weather?lat=52.24749554389182&lon=20.991198645507612&units=metric&appid={apiKey}");
	var json = await response.Content.ReadAsStringAsync();
	var weatherData = JsonSerializer.Deserialize<WeatherResponse>(json, new JsonSerializerOptions
	{ 
		PropertyNameCaseInsensitive = true
	});
	if (!response.IsSuccessStatusCode) {
		return Results.StatusCode((int)response.StatusCode);
	}
	return Results.Ok(new 
	{
		WeatherConditions = weatherData.Weather[0].Description,
		Temperature = weatherData.Main.Temp,
		RealFeel = weatherData.Main.FeelsLike,
		MinTemperature = weatherData.Main.TempMin,
		MaxTemperature = weatherData.Main.TempMax,
		Pressure = weatherData.Main.Pressure,
		Humidity = weatherData.Main.Humidity
	});
});




app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class WeatherResponse { 
	public WeatherData[] Weather { get; set; }
	public MainData Main { get; set; }
}

/*
[{
	"id":300,
	"main":"Drizzle",
	"description":"light intensity drizzle",
	"icon":"09n"
}]
 */
public class WeatherData{ 
	public int Id { get; set; }
	public string Main { get; set; }
	public string Description { get; set; }
	public string Icon { get; set; }
}
/*
	{
		"temp":275.54,
		"feels_like":272.07,
		"temp_min":274.63,
		"temp_max":276.38,
		"pressure":1017,
		"humidity":90,
		"sea_level":1017,
		"grnd_level":1006
	}
 */
public class MainData { 
	public float Temp { get; set; }
	[JsonPropertyName("feels_like")]
	public float FeelsLike { get; set; }
	[JsonPropertyName("temp_min")]
	public float TempMin { get; set; }
	[JsonPropertyName("temp_max")]
	public float TempMax { get; set; }
	public float Pressure { get; set; }
	public float Humidity { get; set; }
	[JsonPropertyName("sea_level")]
	public float SeaLevel { get; set; }
	[JsonPropertyName("grnd_level")]
	public float GrndLevel { get; set; }
}
