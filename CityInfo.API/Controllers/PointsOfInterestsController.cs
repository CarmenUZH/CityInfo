using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController] //Controls that body not null
    [Route("api/cities/{cityId}/pointsofinterest")] //Make REST-full
    public class PointsOfInterestsController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestsController> _logger;
        private readonly IMailService _mailer;
        private readonly ICityInfoRepository _citiesDataStore;
        private readonly IMapper _mapper;

        public PointsOfInterestsController(ILogger<PointsOfInterestsController> logger, IMailService mailer, ICityInfoRepository citiesDataStore, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); //null-check
            _mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(mailer));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId) //Returns empty list if city exists and has no POI's & if city doesnt exist [should cause error]
        {
            if (!await _citiesDataStore.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found. Are you sure this city exists?");
                return NotFound();
            }
            
            var pointsOfInterestForCity = await _citiesDataStore.GetPointOfInterestsForCityAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));

            /* Old implementations
            try
            {
               var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found. Haha just like George."); //Log the information of your failure to the Console so you know
                    return NotFound();
                }
                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}", ex); //we're handling the exception by logging it
                return StatusCode(500, "A problem happened while handling your request."); //IMPORTANT! DON'T EXPOSE TOO MUCH TO YOUR USER
                throw;
        }*/
        }
        [HttpGet("{pointofinterestid}", Name = "GetPOIid")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _citiesDataStore.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found. Are you sure this city exists?");
                return NotFound();
            }

            var pointOfInterest = await _citiesDataStore.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterest));

        }

        [HttpPost] //Posting let's GOOOOOOO
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, POIforCreationDto POI)
        {
            /*
            if (!ModelState.IsValid)
            {
                return BadRequest(); //Gets automatically done by the ApiController
            }*/

            if (!await _citiesDataStore.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found. Are you sure this city exists?");
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(POI);
            await _citiesDataStore.AddPOIForCityAsync(cityId, finalPointOfInterest);
            await _citiesDataStore.SaveChangesAsync();
            var createdPOI = _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPOIid", new //We still have to map ah oof damn
            {
                cityId = cityId, //Id gets generated
                pointOfInterestId = createdPOI.Id
            },
            createdPOI);

        }


        [HttpPut("{pointofinterestid}", Name = "PutPOIid")] //Put should fully update the thing, Patch for partial
        public async Task< ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, POIforUpdateDto POI)
        {
            if (!await _citiesDataStore.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found. Are you sure this city exists?");
                return NotFound();
            }

            var pointOfInterest = await _citiesDataStore.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            _mapper.Map(POI, pointOfInterest);

            point.Name = POI.Name;
            point.Description = POI.Description;
            return NoContent();

        }
        [HttpPatch("{pointofinterestid}", Name = "PatchPOIid")] //Put should fully update the thing, Patch for partial
        public ActionResult PatchPointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<POIforUpdateDto> patchDocument)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }
            var point = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
            if (point == null)
            {
                return NotFound();
            }

            var POIpatch = new POIforUpdateDto()
            {
                Name = point.Name,
                Description = point.Description
            };
            patchDocument.ApplyTo(POIpatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(POIpatch)) //Basically, if your update tries to do something illega (like remove the name) it wont work
            {
                return BadRequest(ModelState);
            }
            point.Name = POIpatch.Name;
            point.Description = POIpatch.Description;
            return NoContent();

        }

        [HttpDelete("{pointOfInterestId}")] //Deleting let's GOOOOOOO, Basel Bahnhof can finally perish
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId) //pointofInterestId needs to have same name to the above https things
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var point = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
            if (point == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(point);
            _mailer.Send("Point of interest deleted", $"Point of interest {point.Name} with id {point.Id} was removed");
            return NoContent();
        }

    }
}
