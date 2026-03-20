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
        public CitiesController(CitiesDataService dataService)
        {
            _dataService = dataService;
        }
        [HttpGet]
        public IActionResult GetAll() {
            return Ok(_dataService.Cities);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var city = _dataService.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null) {
                return NotFound(new { Message = $"City with id: {id} not found." });
            }
            return Ok(city);
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteById(int id)
        {
            var city = _dataService.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return NotFound(new { Message = $"City with id: {id} not found." });
            }
            _dataService.Cities.Remove(city);
            return NoContent();
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateById(int id, [FromBody] City updatedData)
        {
            var city = _dataService.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return NotFound(new { Message = $"City with id: {id} not found." });
            }
            
            var index = _dataService.Cities.IndexOf(city);
            _dataService.Cities[index] = new City { Id = id, Name = updatedData.Name, Country = updatedData.Country };


            return Ok(_dataService.Cities[index]);
        }

        [HttpPost]
        public IActionResult Post(string name, string country)
        {
            var newCity = new Objects.City {Id = _dataService.Cities.Count + 1, Name = name, Country = country };
            _dataService.Cities.Add(newCity);
            return Created("", newCity);
        }
    }
}
