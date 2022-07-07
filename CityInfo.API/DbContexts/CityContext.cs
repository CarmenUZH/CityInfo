using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityContext : DbContext
    {
        public DbSet<City> Cities { get; set; } //Not nullable!!
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }
    }
}
