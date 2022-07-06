using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")] //handles base route
    public class CitiesController: ControllerBase
    {
        private readonly CitiesDataStore _citiesDataStore;

        public CitiesController(CitiesDataStore citiesDataStore)
        {
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore)); 
        }

        [HttpGet] //Specify routes with attributes (see Program.cs) - Here you dont need to input a specific route because its the base route!
        //you can input [HttpGet("api/[controller]")] because the name matches (refactoring the name would cause problems later tho!)
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {

           return Ok(
             _citiesDataStore.Cities );
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id) //Return an actionresult and not something like Json because this makes us Independent
        {
            var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);
           //FirstOrDefault returns first match or default value, will return "null" for nonexistant values
           if(cityToReturn == null)
            {
                return NotFound();
            }
           return Ok(cityToReturn); //Return city with 200ok statues
        }
    }
}
