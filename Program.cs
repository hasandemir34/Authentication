using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using uyesistemi.Data; // Kendi oluşturduğun DbContext'in olduğu klasör

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("IdentityDbConnection")));
// Add services to the container.
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UygulamaDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication(); // 1. Kapı: "Sen kimsin? Giriş yaptın mı?" [cite: 6, 94]
app.UseAuthorization();  // 2. Kapı: "Giriş yapmışsın ama buraya girmeye yetkin var mı?" [cite: 9, 95]

app.UseAuthorization();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
