using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;//Not nullable!! DbContext handles that in background
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

       public CityContext(DbContextOptions<CityContext> options) : base(options)
        {

        }
    }
}
