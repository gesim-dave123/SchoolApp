using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using SchoolApp.Data;

var builder = WebApplication.CreateBuilder(args);

// SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=schoolapp.db"));

// ✅ Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Auto-create DB + seed default admin
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    // ✅ Seed a default admin account if no users exist
    if (!db.Users.Any())
    {
        db.Users.Add(new SchoolApp.Models.UserModel
        {
            Name = "Administrator",
            Email = "admin@school.com",
            Password = "Admin123",
            Age = 25,
            Role = "admin"
        });
        db.SaveChanges();
    }
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();  // ✅ Must be before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Landing}/{id?}"); // ✅ Start at Landing page

app.Run();