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
        [HttpGet("{pointofinterestid}", Name = "GetPOIid")] //NAMES NEED TO BE SAME (pointofinterestid)
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointofinterestid)
        {
            if (!await _citiesDataStore.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found. Are you sure this city exists?");
                return NotFound();
            }

            var pointOfInterest = await _citiesDataStore.GetPointOfInterestForCityAsync(cityId, pointofinterestid);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));

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
            await _citiesDataStore.AddPOIForCityAsync(cityId, finalPointOfInterest); //In die Datenbank
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
        public async Task< ActionResult> UpdatePointOfInterest(int cityId, int pointofinterestid, POIforUpdateDto POI)
        {
            if (!await _citiesDataStore.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found. Are you sure this city exists?");
                return NotFound();
            }

            var pointOfInterest = await _citiesDataStore.GetPointOfInterestForCityAsync(cityId, pointofinterestid);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            _mapper.Map(POI, pointOfInterest);
            await _citiesDataStore.SaveChangesAsync();
            return NoContent();

        }
        [HttpPatch("{pointofinterestid}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(
          int cityId, int pointofinterestid,
          JsonPatchDocument<POIforUpdateDto> patchDocument)
        {
            if (!await _citiesDataStore.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _citiesDataStore
                .GetPointOfInterestForCityAsync(cityId, pointofinterestid);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<POIforUpdateDto>(
                pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            await _citiesDataStore.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointofinterestid}")]
        public async Task<ActionResult> DeletePointOfInterest(
            int cityId, int pointofinterestid)
        {
            if (!await _citiesDataStore.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _citiesDataStore
                .GetPointOfInterestForCityAsync(cityId, pointofinterestid);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _citiesDataStore.DeletePointOfInterest(pointOfInterestEntity); //CHECK TUTORIAL
            await _citiesDataStore.SaveChangesAsync();

            _mailer.Send(
                "Point of interest deleted.",
                $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

            return NoContent();
        }


    }
}
