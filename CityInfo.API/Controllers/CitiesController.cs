using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    public class CitiesController: ControllerBase
    {
        [HttpGet("api/[controller]")] //Specify routes with attributes (see Program.cs)
        //you can input controller because the name matches (refactoring the name would cause problems later tho!)
        public JsonResult GetCities()
        {
           return new JsonResult(
                new List<object>
                {
                    new { id = 1, Name="Basel" },
                    new{id = 2, Name="Bern"} 
                });
        }
    }
}
