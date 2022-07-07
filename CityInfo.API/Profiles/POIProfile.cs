using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class POIProfile: Profile
    {
        public POIProfile()
        {
            //It's not much but it's honest work
            CreateMap<Entities.PointOfInterest,Models.PointOfInterestDto>();
            CreateMap<Models.POIforCreationDto, Entities.PointOfInterest>();
            CreateMap<Models.POIforUpdateDto, Entities.PointOfInterest>();

        }
    }
}
