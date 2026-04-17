using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarListApp.Api
{
    public class CarListDbContext : IdentityDbContext
    {
        public CarListDbContext(DbContextOptions<CarListDbContext> options) : base(options)
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

            // 1. ΔΙΟΡΘΩΣΗ: Εδώ πρέπει να είναι IdentityRole, όχι IdentityUser
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "d1b1a5e8-9c3b-4f1a-8c2e-424gkp3rbv71",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Id = "j2h455en-1y8m-6w2c-7r7s-3fk450j963n2",
                    Name = "User",
                    NormalizedName = "USER"
                }
            );

            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    UserName = "admin@localhost.com",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                    EmailConfirmed = true,
                    // ΠΡΟΣΘΕΣΕ ΑΥΤΑ:
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    AccessFailedCount = 0,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true
                },
                new IdentityUser
                {
                    Id = "p6o5n4m3-l2k1-j0i9-h8g7-f6e5d4c3b2a1",
                    Email = "user@localhost.com",
                    NormalizedEmail = "USER@LOCALHOST.COM",
                    NormalizedUserName = "USER@LOCALHOST.COM",
                    UserName = "user@localhost.com",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                    EmailConfirmed = true,
                    // ΚΑΙ ΕΔΩ:
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    AccessFailedCount = 0,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6",
                    RoleId = "d1b1a5e8-9c3b-4f1a-8c2e-424gkp3rbv71"
                },
                new IdentityUserRole<string>
                {
                    UserId = "p6o5n4m3-l2k1-j0i9-h8g7-f6e5d4c3b2a1",
                    RoleId = "j2h455en-1y8m-6w2c-7r7s-3fk450j963n2"
                }
            );
        }
    }
}