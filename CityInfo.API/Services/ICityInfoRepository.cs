using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {//Only include the methods you actually use
        Task<IEnumerable<City>> GetCitiesAsync(); //Async for scalability improvements
        Task<City?> GetCityAsync(int cityId,bool includePOI);
        Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId);
        Task<bool> CityExistsAsync(int cityId);
        Task<PointOfInterest?>GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
        Task AddPOIForCityAsync(int cityId, PointOfInterest POI);
        Task<bool> SaveChangesAsync();
    }
}
