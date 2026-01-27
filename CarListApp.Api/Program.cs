
using System.Runtime.ConstrainedExecution;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace CarListApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", a => a.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            });
            var dbPath = Path.Join(Directory.GetCurrentDirectory(), "carlist.db");
            var conn = new SqliteConnection("Data Source=carlist.db");
            builder.Services.AddDbContext<CarListDbContext>(o => o.UseSqlite(conn));
            var app = builder.Build();

            app.MapOpenApi();
            app.MapScalarApiReference();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.MapGet("/cars", async (CarListDbContext db) => await db.Cars.ToListAsync());
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

            app.MapPost("/cars", async (int id, Car car, CarListDbContext db) => {
                await db.AddAsync(car);
                await db.SaveChangesAsync();

                return Results.Created($"/cars/{car.Id}", car);
            });

            app.Run();
        }
    }
}
