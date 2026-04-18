using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using CarListApp.Api;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});

/*builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});*/

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
app.UseAuthentication();
app.UseAuthorization();


// --- ENDPOINTS ---

// Get all Cars
app.MapGet("/cars", async (CarListDbContext db) => await db.Cars.ToListAsync());

// Get Car by ID
app.MapGet("/cars/{id}", async (int id, CarListDbContext db) =>
    await db.Cars.FindAsync(id) is Car car ? Results.Ok(car) : Results.NotFound()
);

// Update Car
app.MapPut("/cars/{id}", async (int id, Car car, CarListDbContext db) => {
    var record = await db.Cars.FindAsync(id);
    if (record is null) return Results.NotFound();

    record.Make = car.Make;
    record.Model = car.Model;
    record.Vin = car.Vin;

    await db.SaveChangesAsync();

    return Results.NoContent();

});

// Delete Car
app.MapDelete("/cars/{id}", async (int id, CarListDbContext db) => {
    var record = await db.Cars.FindAsync(id);
    if (record is null) return Results.NotFound();
    db.Remove(record);
    await db.SaveChangesAsync();

    return Results.NoContent();

});

// Create Car
app.MapPost("/cars", async (Car car, CarListDbContext db) => {
    await db.AddAsync(car);
    await db.SaveChangesAsync();

    return Results.Created($"/cars/{car.Id}", car);

});


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

    //Generate an access token
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var roles = await _userManager.GetRolesAsync(user);
    var claims = await _userManager.GetClaimsAsync(user);
    var tokenClaims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim("email_confirmed", user.EmailConfirmed.ToString())

    }.Union(claims)
    .Union(roles.Select(role => new Claim(ClaimTypes.Role, role)));

    var token = new JwtSecurityToken(
        issuer: builder.Configuration["JwtSettings:Issuer"],
        audience: builder.Configuration["JwtSettings:Audience"],
        claims: tokenClaims,
        expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(builder.Configuration["JwtSettings:DurationInMinutes"])),
        signingCredentials: credentials
    );

    var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

    var response = new AuthResponseDto
    {
        UserId = user.Id,
        Username = user.UserName,
        Token = accessToken
    };

    return Results.Ok(response);
}).AllowAnonymous();


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