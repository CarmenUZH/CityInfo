using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class CityProfile:Profile
    {
        public CityProfile()
        {
            CreateMap<Entities.City, Models.CityNoPOIDto>(); //Establish what to map
            CreateMap<Entities.City, Models.CityDto>(); //Establish what to map

        }
    }
}
