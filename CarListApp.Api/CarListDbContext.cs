using Microsoft.EntityFrameworkCore;

namespace CarListApp.Api
{
    public class CarListDbContext : DbContext
    {
        public CarListDbContext(DbContextOptions<CarListDbContext>options) : base(options)
        {
            
        }

        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    Id = 1,
                    Make = "Toyota",
                    Model = "Corolla",
                    Vin = "JT4BG22K6V000001"

                },
                new Car
                {
                    Id = 2,
                    Make = "Honda",
                    Model = "Civic",
                    Vin = "2HGES16555H000002"
                },
                new Car
                {
                    Id = 3,
                    Make = "Ford",
                    Model = "Mustang",
                    Vin = "1FAFP404X1F000003"
                },
                new Car
                {
                    Id = 4,
                    Make = "Chevrolet",
                    Model = "Impala",
                    Vin = "2G1WF52E659000004"
                },
                new Car
                {
                    Id = 5,
                    Make = "Nissan",
                    Model = "Altima",
                    Vin = "1N4AL11D75C000005"
                },
                new Car
                {
                    Id = 6,
                    Make = "BMW",
                    Model = "320",
                    Vin = "WBA3A5C50DF000006"
                },
                new Car
                {
                    Id = 7,
                    Make = "Audi",
                    Model = "A4",
                    Vin = "WAUDF78E37A000007"
                },
                new Car
                {
                    Id = 8,
                    Make = "Mercedes-Benz",
                    Model = "C-Class",
                    Vin = "WDBRF40JX3F000008"
                },
                new Car
                {
                    Id = 9,
                    Make = "Volkswagen",
                    Model = "Jetta",
                    Vin = "3VW2K7AJ5DM000009"
                },
                new Car
                {
                    Id = 10,
                    Make = "Subaru",
                    Model = "Impreza",
                    Vin = "JF1GPAL69DH000010"
                }







            );

        }
    }
}