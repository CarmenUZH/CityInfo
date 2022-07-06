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
                    Description="Hab meine Ehre dort verloren",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 3,
                            Name = "Zoo Basel",
                            Description = "Wo ich meine Ehre verloren habe"
                        },
                          new PointOfInterestDto()
                        {
                            Id = 4,
                            Name = "Bahnhof Basel",
                            Description = "Alter, wie einfach das erste was man sieht andere Orte ist. Like chill, ich weiss ja dass ich nicht HIER sein will"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Bern",
                    Description = "Tut so als hätten sie Einstein als homie gehabt",
                     PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 5,
                            Name = "Uni Bern",
                            Description = "Hatten wetten im Lockdown noch präsenzunterricht"
                        },
                          new PointOfInterestDto()
                        {
                            Id = 6,
                            Name = "Museum für Kommunikation",
                            Description = "Actually nice"
                        }
                    }
                }
            };
        }
    }
}
