using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityRepository : ICityInfoRepository
    {
        private readonly CityContext _context;

        public CityRepository(CityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddPOIForCityAsync(int cityId, PointOfInterest POI)
        {
            var city = await GetCityAsync(cityId, false);
            if (city != null)
            {
                city.PointsOfInterest.Add(POI); //Alone doesnt persist POI, only adds it to inmemory representation of Database
            }
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0); //Saves the changes, like our commit in OdeToFood
        }

        public async Task<bool> CityExistsAsync(int cityId) //Easier way to determine if city even exists
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }


        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c=> c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePOI)
        {
            if (includePOI)
            {
                return await _context.Cities.Include(c => c.PointsOfInterest).Where(c=>c.Id == cityId).FirstOrDefaultAsync();
            }
            return await _context.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await _context.PointsOfInterest.Where(c => c.CityId == cityId && c.Id == pointOfInterestId).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId)
        {
            return await _context.PointsOfInterest.Where(c => c.CityId == cityId).ToListAsync();

        }
    }
}
