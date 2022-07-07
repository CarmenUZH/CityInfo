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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("Basel")
                {
                    Id = 1,
                    Description = "Wo ich meine Ehre verloren hab"
                },
                new City("Bern")
                {
                    Id = 2,
                    Description = "Ultra hübsch"
                },
                new City("Zürich")
                {
                    Id = 3,
                    Description = "Hat Oerlikon LMAO"
                }
                );
            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Zoo Basel")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "Wo ich meine Ehre verloren hab"
                },
                new PointOfInterest("Meine Ehre")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "Wait, ist das dort meine Ehre??"
                },
                 new PointOfInterest("Uni Bern")
                 {
                     Id = 3,
                     CityId = 2,
                     Description = "Tun so als wären sie ultra homies mit Einstein"
                 },
                new PointOfInterest("Rinesas Ehre")
                {
                    Id = 4,
                    CityId = 2,
                    Description = "Loser muss immer lernen"
                },
                 new PointOfInterest("Oerlikon")
                 {
                     Id = 5,
                     CityId = 3,
                     Description = "LMAO"
                 },
                new PointOfInterest("Töbes Ehre")
                {
                    Id = 6,
                    CityId = 3,
                    Description = "Zürich ist verbrannt, whoops"
                }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
