var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
	"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
	var forecast = Enumerable.Range(1, 5).Select(index =>
		new WeatherForecast
		(
			DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
			Random.Shared.Next(-20, 55),
			summaries[Random.Shared.Next(summaries.Length)]
		))
		.ToArray();
	return forecast;
});

app.MapGet("/currentTemperature", () =>
{
	return Results.Ok(new { currentTemperature = new Random().Next(-35,35) });
});
app.MapGet("/currentWindDirection", () => {
	List<string> windDirections = new List<string>
		{
			"North",
			"South",
			"East",
			"West",
			"North-East",
			"North-West",
			"South-East",
			"South-West"
		};

	int index = new Random().Next(windDirections.Count);

	return Results.Ok(new {currentWindDirection = windDirections[index] });

});



app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
