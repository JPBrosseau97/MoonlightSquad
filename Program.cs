using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MoonlightSquad.Class.BLL;
using MoonlightSquad.Class.DAL;

var builder = WebApplication.CreateBuilder(args);

//=====================================
//SERVICES
//=====================================
builder.Services.AddRazorPages();

// Base de données SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=moonlightsquad.db"));

// Service utilisateurs
builder.Services.AddScoped<UserService>();

// Authentification par cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

var app = builder.Build();

// Initialisation de la base de données au démarrage
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    var userService = scope.ServiceProvider.GetRequiredService<UserService>();
    await userService.SeedAdminAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
