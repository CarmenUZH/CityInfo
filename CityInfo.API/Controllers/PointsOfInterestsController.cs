using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
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
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestsController(ILogger<PointsOfInterestsController> logger, IMailService mailer,CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); //null-check
            _mailer = mailer ?? throw new ArgumentNullException(nameof(mailer));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(mailer));
        }


        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
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
            }
            }
        [HttpGet("{pointofinterestid}", Name = "GetPOIid")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }
            var point = city.PointsOfInterest.FirstOrDefault(c=> c.Id == pointOfInterestId);
            if ( point == null)
            {
                return NotFound();
            }
            return Ok(point);

        }

        [HttpPost] //Posting let's GOOOOOOO
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, POIforCreationDto POI)
        {
            /*
            if (!ModelState.IsValid)
            {
                return BadRequest(); //Gets automatically done by the ApiController
            }*/

            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            //Temporary solution that should be improved
            var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = POI.Name,
                Description = POI.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPOIid", 
                new { 
                    cityId = cityId, 
                    pointOfInterestId = finalPointOfInterest.Id 
                }, finalPointOfInterest);
        }


        [HttpPut("{pointofinterestid}", Name = "PutPOIid")] //Put should fully update the thing, Patch for partial
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, POIforUpdateDto POI)
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
