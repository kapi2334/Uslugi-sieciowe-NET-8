using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();



/*
	Returns the weather forecast for the specified city for the next 3 hours. 
	Accepts the "?city=name" parameter, which defines the name of the city for which the weather forecast is to be retrieved.
 */
app.MapGet("/weatherforecast", async Task<IResult> (string? city) =>
{
	if (city == null) {
		return Results.BadRequest("city param is requierd.");
	}
	//Lat & lon for Warsaw, 
	
	var handler = new HttpClientHandler();
	using var httpClient = new HttpClient(handler)
	{
		BaseAddress = new Uri("https://api.openweathermap.org/")
	};
	/*
		Paste your openwheathermap API key to the variable below.
	 */
	string apiKey = "PASTE_YOUR_API_KEY_HERE";
	using HttpResponseMessage response = await httpClient.GetAsync($"data/2.5/forecast?q={city}&units=metric&lang=pl&appid={apiKey}");
	var json = await response.Content.ReadAsStringAsync();
	//return Results.Ok(json);
	var weatherData = JsonSerializer.Deserialize<WeatherForecast>(json, new JsonSerializerOptions
	{
		PropertyNameCaseInsensitive = true
	});
	if (!response.IsSuccessStatusCode)
	{
		return Results.Problem(
			statusCode: (int)response.StatusCode
			);
	}

	ForecastItem firstWeatherData;
	if (weatherData == null || weatherData.Forecasts == null)
	{
		return Results.Problem(
			detail: "API returned invalid forecast array. Check if provided city name is valid!",
			statusCode: 502
			);
	}
	else {
		firstWeatherData = weatherData.Forecasts[0];
	}
	
	

	return Results.Ok(new
	{
			ForecastTime = DateTimeOffset.FromUnixTimeSeconds(firstWeatherData.UnixDateTime).LocalDateTime,
			Temp = firstWeatherData.Main.Temp,
			FeelsLike = firstWeatherData.Main.FeelsLike,
			TempMin = firstWeatherData.Main.TempMin,
			TempMax = firstWeatherData.Main.TempMax,
			Pressure = firstWeatherData.Main.Pressure,
			Humidity = firstWeatherData.Main.Humidity,
			Description = firstWeatherData.Weather[0].Description,
			WindSpeed = firstWeatherData.Wind.Speed,
			Visibility = firstWeatherData.Visibility,
			Rain3hVolume = firstWeatherData.Rain?.Volume3h,
			Snow3hVolume = firstWeatherData.Snow?.Volume3h
	});
});




app.Run();

public class WeatherForecast {
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


