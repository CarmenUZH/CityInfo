using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")] //handles base route
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _citiesDataStore; //Too lazy to change name
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository citiesDataStore, IMapper mapper)
        {
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet] //Specify routes with attributes (see Program.cs) - Here you dont need to input a specific route because its the base route!
        //you can input [HttpGet("api/[controller]")] because the name matches (refactoring the name would cause problems later tho!)
        public async Task<ActionResult<IEnumerable<CityNoPOIDto>>> GetCities() //When async dont forget Task
        {
            var cityEntities = await _citiesDataStore.GetCitiesAsync(); //Our API wants returns that are like the Models defined in the Model folder but we now use a repository pattern which means we have to change our repository result to a DTO
            return Ok(_mapper.Map<IEnumerable<CityNoPOIDto>>(cityEntities)); //Automatically maps for us
           // return Ok( _citiesDataStore.Cities);
        }

       /* [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id) //Return an actionresult and not something like Json because this makes us Independent
        {
            var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);
            //FirstOrDefault returns first match or default value, will return "null" for nonexistant values
            if (cityToReturn == null)
            {
                return NotFound();
            }
            return Ok(cityToReturn); //Return city with 200ok statues
        } */
    }
}
