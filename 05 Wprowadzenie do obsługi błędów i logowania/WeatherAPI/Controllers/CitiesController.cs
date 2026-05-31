using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Objects;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController: ControllerBase
    {
        private readonly CitiesDataService _dataService;
        private readonly ILogger<CitiesController> _logger;
        public CitiesController(CitiesDataService dataService, ILogger<CitiesController> logger)
        {
            _logger = logger;
            _dataService = dataService;
        }
        [HttpGet]
        public IActionResult GetAll() {
            _logger.LogInformation("Downloading data about all cities.");
            return Ok(_dataService.Cities);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var city = _dataService.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null) {
                _logger.LogError("Failed to obtain data about city no.{cityId}.", id);
                return NotFound(new { Message = $"City with id: {id} not found." });
            }
            _logger.LogInformation("Obtaining data about city no.{cityId}.", id);
            return Ok(city);
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteById(int id)
        {
            var city = _dataService.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                _logger.LogError("Failed to delete city's data with id:{cityId}.", id);
                return NotFound(new { Message = $"City with id: {id} not found." });
            }
            _dataService.Cities.Remove(city);
            _logger.LogInformation("Deleting data about city no.{cityId}.", id);
            return NoContent();
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateById(int id, [FromBody] City updatedData)
        {
            var city = _dataService.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                _logger.LogError("Failed to update city's data with id:{cityId}.", id);
                return NotFound(new { Message = $"City with id: {id} not found." });
            }
            
            var index = _dataService.Cities.IndexOf(city);
            _dataService.Cities[index] = new City { Id = id, Name = updatedData.Name, Country = updatedData.Country };
            _logger.LogInformation("Upadting data about city no.{cityId}.", id);


            return Ok(_dataService.Cities[index]);
        }

        [HttpPost]
        public IActionResult Post(string name, string country)
        {
            var newCity = new Objects.City {Id = _dataService.Cities.Count + 1, Name = name, Country = country };
            _dataService.Cities.Add(newCity);
            _logger.LogInformation("Created new city with id {newCityId}.", _dataService.Cities.Count);
            return Created("", newCity);

        }
    }
}
