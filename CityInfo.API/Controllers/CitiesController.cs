using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")] //handles base route
    public class CitiesController: ControllerBase
    {
        [HttpGet] //Specify routes with attributes (see Program.cs) - Here you dont need to input a specific route because its the base route!
        //you can input [HttpGet("api/[controller]")] because the name matches (refactoring the name would cause problems later tho!)
        public JsonResult GetCities()
        {
           return new JsonResult(
              CitiesDataStore.Current.Cities );
        }

        [HttpGet("{id}")]
        public JsonResult GetCity(int id)
        {
            return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id)); //FirstOrDefault returns first match or default value
        }
    }
}
