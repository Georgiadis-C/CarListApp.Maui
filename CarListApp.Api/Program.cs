using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using CarListApp.Api;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// --- ΡΥΘΜΙΣΕΙΣ ΥΠΗΡΕΣΙΩΝ ---
builder.WebHost.UseUrls("http://0.0.0.0:5069");

builder.Services.AddAuthorization();
builder.Services.AddOpenApi();

builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAll", a => a.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

// Ρύθμιση SQLite
var dbPath = Path.Join(Directory.GetCurrentDirectory(), "carlist.db");
var conn = new SqliteConnection($"Data Source={dbPath}");
builder.Services.AddDbContext<CarListDbContext>(o => o.UseSqlite(conn));

// Ρύθμιση Identity
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CarListDbContext>();

var app = builder.Build();

// --- ΑΥΤΟΜΑΤΟ SETUP ΒΑΣΗΣ & ADMIN (SEEDING) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CarListDbContext>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        // Το EnsureCreated φτιάχνει τους πίνακες αμέσως αν λείπουν
        context.Database.EnsureCreated();

        var adminEmail = "admin@localhost.com";
        // Χρησιμοποιούμε Task.Run για να εκτελεστεί σωστά η async μέθοδος στην εκκίνηση
        var user = Task.Run(() => userManager.FindByNameAsync(adminEmail)).Result;

        if (user == null)
        {
            var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
            var result = Task.Run(() => userManager.CreateAsync(adminUser, "P@ssword1")).Result;

            if (result.Succeeded)
            {
                Console.WriteLine(">>> Η βάση δημιουργήθηκε και ο Admin προστέθηκε επιτυχώς!");
            }
        }
        else
        {
            Console.WriteLine(">>> Η βάση είναι έτοιμη και ο Admin υπάρχει ήδη.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] Πρόβλημα κατά την εκκίνηση της βάσης: {ex.Message}");
    }
}

// --- MIDDLEWARE ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowAll");
app.UseAuthorization();

// --- ENDPOINTS ---

// 1. Get all cars
app.MapGet("/cars", async (CarListDbContext db) =>
    await db.Cars.ToListAsync());

// 2. Login
app.MapPost("/login", async (LoginDto loginDto, UserManager<IdentityUser> _userManager) =>
{
    if (loginDto == null || string.IsNullOrEmpty(loginDto.Username))
        return Results.BadRequest("Missing credentials");

    var user = await _userManager.FindByNameAsync(loginDto.Username);
    if (user is null)
        return Results.Unauthorized();

    var isValidPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);

    if (!isValidPassword)
        return Results.Unauthorized();

    return Results.Ok(new AuthResponseDto
    {
        UserId = user.Id,
        Username = user.UserName,
        Token = "Success_Token_123"
    });
});

// 3. Post car
app.MapPost("/cars", async (Car car, CarListDbContext db) => {
    await db.Cars.AddAsync(car);
    await db.SaveChangesAsync();
    return Results.Created($"/cars/{car.Id}", car);
});

app.Run();

// --- DTO CLASSES ---
public class LoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}