
using Microsoft.EntityFrameworkCore;
using HalloDocRepository.DataModels;
using HalloDocRepository.Implementation;
using HalloDocRepository.Interfaces;
using HalloDocService.Implementation;
using HalloDocService.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using HalloDocRepository.Admin.Interfaces;
using HalloDocRepository.Admin.Implementation;
using HalloDocService.Admin.Interfaces;
using HalloDocService.Admin.Implementation;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HalloDocContext>(q => q.UseNpgsql(conn));
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.AreaPageViewLocationFormats.Add("Views/{1}/{0}.cshtml");
    options.AreaPageViewLocationFormats.Add("Views/Shared/{0}.cshtml");
    options.AreaPageViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
    options.AreaPageViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
});
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPatientLoginRepo, PatientLoginRepo>();
builder.Services.AddScoped<IPatientLogin, PatientLogin>();
builder.Services.AddScoped<IPatientRequestRepo, PatientRequestRepo>();
builder.Services.AddScoped<IPatientRequestService, PatientRequestService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDashboardRepo, DashboardRepo>();
builder.Services.AddScoped<IAdminDashboardRepo, AdminDashboardRepo>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IUtilityService, UtilityService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                if (context.Request.Path.StartsWithSegments("/Admin") &&
                    !context.Request.Path.StartsWithSegments("/Admin/Login"))
                {
                    context.Response.Redirect("/Admin/Login/Index");
                }
                else if (context.Request.Path.StartsWithSegments("/Patient") &&
                         !context.Request.Path.StartsWithSegments("/Patient/PatientLogin"))
                {
                    context.Response.Redirect("/Patient/PatientLogin/Index");
                }
                else
                {
                    context.Response.Redirect("/Account/Login");
                }
                return Task.CompletedTask;
            }
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Redirect root URL to your desired default URL
app.Use(async (context, next) =>
{

    ClaimsPrincipal claimUser = context.User;
    if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest" && claimUser.Identity.IsAuthenticated==false){
        Console.WriteLine("In this Middlewear");
        context.Response.StatusCode = 401;
        return;
    }

    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/Patient/Home/Index");
        return;
    }

    await next();
    
     if(context.Response.StatusCode == 403 || context.Response.StatusCode == 404){
        context.Response.Redirect("/Account/NotFound");
        return;
    }

    
});


app.MapControllerRoute(
    name: "Patient",
    pattern: "{area=exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
       name: "Admin",
        pattern: "{area=exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=AccessDenied}/{id?}");

app.Run();