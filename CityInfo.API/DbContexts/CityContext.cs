using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;//Not nullable!! DbContext handles that in background
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(); //What database to use
            base.OnConfiguring(optionsBuilder);
        }
    }
}
