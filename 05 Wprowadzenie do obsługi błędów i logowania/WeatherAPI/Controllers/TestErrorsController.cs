using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Controllers;


[Route("api/[controller]")]
[ApiController]
public class TestErrorsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        throw new Exception("Test exception");
    }
}