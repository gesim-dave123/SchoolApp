using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register SQLite with EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=schoolapp.db"));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ✅ Auto-create the database on startup (no migration command needed)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();