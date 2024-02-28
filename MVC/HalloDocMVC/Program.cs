
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
using Microsoft.AspNetCore.Mvc;

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

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Option =>
{
    Option.LoginPath = "/PatientLogin/Index";
    Option.ExpireTimeSpan = TimeSpan.FromDays(7);
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

app.MapControllerRoute(
       name: "Admin",
        pattern: "{area=exists}/{controller=Dashboard}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();