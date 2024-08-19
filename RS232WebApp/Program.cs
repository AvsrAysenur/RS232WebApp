using Microsoft.EntityFrameworkCore;
using RS232WebApp.Data;
using RS232WebApp.Models;


var builder = WebApplication.CreateBuilder(args);

// Veritaban� ba�lanmas�n� yap�land�rma
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// RS232 servisini yap�land�rma
builder.Services.AddSingleton(new RS232Service("COM1", 9600, "COM3", 9600));

// MVC yap�land�rma
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
