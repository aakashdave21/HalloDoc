
using Microsoft.EntityFrameworkCore;
using HalloDocRepository.DataModels;
using Microsoft.AspNetCore.Mvc.Razor;
using HalloDocMVC.Extensions;
using HalloDocMVC.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HalloDocContext>(q => q.UseNpgsql(conn));
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.AreaPageViewLocationFormats.Add("Views/{1}/{0}.cshtml");
    options.AreaPageViewLocationFormats.Add("Views/Shared/{0}.cshtml");
    options.AreaPageViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
    options.AreaPageViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
});
builder.Services.AddControllersWithViews(options =>
{
    // Added Filters For Access
    options.Filters.Add<MenuItemsActionFilter>();
});

// Json Serialization Cycle detect Prevention
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// Scoped/Custom Auth Are Added From Extensions
builder.Services.AddMyServices();
builder.Services.AddMyAuthentication();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();



// Custom Middleware Related To Authenticatin
app.UseMyMiddleware();

app.MapHub<ChatHub>("/chatHub");


app.MapControllerRoute(
       name: "Admin",
        pattern: "{area=exists}/{controller=Dashboard}/{action=Index}/{id?}");
        
app.MapControllerRoute(
       name: "Provider",
        pattern: "{area=exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Patient",
    pattern: "{area=exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=AccessDenied}/{id?}");

app.Run();