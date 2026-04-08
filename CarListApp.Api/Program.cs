using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using CarListApp.Api; // Βεβαιώσου ότι το namespace είναι το σωστό για το δικό σου project

namespace CarListApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ΔΙΟΡΘΩΣΗ 1: Λέμε στο API να "ακούει" σε όλες τις διευθύνσεις (0.0.0.0) 
            // και όχι μόνο στο localhost, ώστε να το βλέπει ο Emulator.
            builder.WebHost.UseUrls("http://0.0.0.0:5069");

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddOpenApi();

            builder.Services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", a => a.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            });

            // Ρύθμιση της βάσης δεδομένων
            var dbPath = Path.Join(Directory.GetCurrentDirectory(), "carlist.db");
            var conn = new SqliteConnection($"Data Source={dbPath}");
            builder.Services.AddDbContext<CarListDbContext>(o => o.UseSqlite(conn));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            // ΔΙΟΡΘΩΣΗ 2: ΑΠΕΝΕΡΓΟΠΟΙΟΥΜΕ το HttpsRedirection.
            // Αυτό εμποδίζει το API να σε αναγκάζει να πας σε HTTPS (που μπλοκάρει το Android).
            // app.UseHttpsRedirection(); 

            app.UseCors("AllowAll");
            app.UseAuthorization();

            // --- ENDPOINTS ---

            app.MapGet("/cars", async (CarListDbContext db) =>
                await db.Cars.ToListAsync());

            app.MapGet("/cars/{id}", async (int id, CarListDbContext db) =>
                await db.Cars.FindAsync(id) is Car car ? Results.Ok(car) : Results.NotFound()
            );

            app.MapPut("/cars/{id}", async (int id, Car car, CarListDbContext db) => {
                var record = await db.Cars.FindAsync(id);
                if (record is null) return Results.NotFound();

                record.Make = car.Make;
                record.Model = car.Model;
                record.Vin = car.Vin;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            app.MapDelete("/cars/{id}", async (int id, CarListDbContext db) => {
                var record = await db.Cars.FindAsync(id);
                if (record is null) return Results.NotFound();
                db.Cars.Remove(record);

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            app.MapPost("/cars", async (Car car, CarListDbContext db) => {
                await db.AddAsync(car);
                await db.SaveChangesAsync();
                return Results.Created($"/cars/{car.Id}", car);
            });

            app.Run();
        }
    }
}