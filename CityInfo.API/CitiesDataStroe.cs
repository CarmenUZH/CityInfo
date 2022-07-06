using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        public static CitiesDataStore Current { get; } = new CitiesDataStore(); //singleton, work on same store
        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name="Basel",
                    Description="Hab meine Ehre dort verloren"
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Bern",
                    Description = "Tut so als hätten sie Einstein als homie gehabt"
                }
            };
        }
    }
}
