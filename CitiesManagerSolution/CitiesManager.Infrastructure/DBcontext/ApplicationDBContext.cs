using Microsoft.EntityFrameworkCore;
using CitiesManager.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CitiesManager.Core.Entities.IdentityUser;

namespace CitiesManager.Infrastructure.DBcontext
{
    public class ApplicationDBContext: IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().HasData(
                new City
                {
                    CityId = new Guid("4eb4c0ff-b77e-449f-908b-36a719d0ba46"),
                    CityName = "New York"
                },
                new City
                {
                    CityId = new Guid("a269449f-b88f-47b8-9c0e-75c40bdaf66e"),
                    CityName = "Los Angeles"
                },
                new City
                {
                    CityId = new Guid("f5c3f77a-9af8-4c8c-a16c-4eda01bcae03"),
                    CityName = "Chicago"
                }
            );
        }
    }
}
