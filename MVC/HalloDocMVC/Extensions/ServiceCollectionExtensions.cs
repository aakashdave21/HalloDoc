using HalloDocRepository.Interfaces;
using HalloDocRepository.Implementation;
using HalloDocService.Interfaces;
using HalloDocService.Implementation;
using HalloDocService.Provider.Interfaces;
using HalloDocService.Provider.Implementation;
using HalloDocRepository.Provider.Interfaces;
using HalloDocRepository.Provider.Implementation;
using HalloDocRepository.Admin.Interfaces;
using HalloDocRepository.Admin.Implementation;
using HalloDocService.Admin.Interfaces;
using HalloDocService.Admin.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace HalloDocMVC.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyServices(this IServiceCollection services)
        {
            services.AddScoped<IPatientLoginRepo, PatientLoginRepo>();
            services.AddScoped<IPatientLogin, PatientLogin>();
            services.AddScoped<IPatientRequestRepo, PatientRequestRepo>();
            services.AddScoped<IPatientRequestService, PatientRequestService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IDashboardRepo, DashboardRepo>();
            services.AddScoped<IAdminDashboardRepo, AdminDashboardRepo>();
            services.AddScoped<IAdminDashboardService, AdminDashboardService>();
            services.AddScoped<IUtilityService, UtilityService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IProfileRepo, ProfileRepo>();
            services.AddScoped<IProviderService, ProviderService>();
            services.AddScoped<IProviderRepo, ProviderRepo>();
            services.AddScoped<IAccessService, AccessService>();
            services.AddScoped<IAccessRepo, AccessRepo>();
            services.AddScoped<IScheduleRepo, ScheduleRepo>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IPartnerRepo, PartnerRepo>();
            services.AddScoped<IRecordsService, RecordsService>();
            services.AddScoped<IRecordsRepo, RecordsRepo>();
            services.AddScoped<IProviderDashboardService, ProviderDashboardService>();
            services.AddScoped<IProviderDashboardRepo, ProviderDashboardRepo>();
            services.AddScoped<IProviderInvoicingService, ProviderInvoicingService>();
            services.AddScoped<IProviderInvoicingRepo, ProviderInvoicingRepo>();
            return services;
        }

        public static IServiceCollection AddMyAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromHours(24);
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            var httpContext = context.HttpContext;
                            if (!httpContext.User.Identity.IsAuthenticated && context.Request.Path.StartsWithSegments("/Provider") && !context.Request.Path.StartsWithSegments("/Provider/Login"))
                            {
                                context.Response.Redirect("/Provider/Login/");
                            }
                            else if (!httpContext.User.Identity.IsAuthenticated && context.Request.Path.StartsWithSegments("/Admin") &&
                                !context.Request.Path.StartsWithSegments("/Admin/Login"))
                            {
                                context.Response.Redirect("/Admin/Login/Index");
                            }
                            else if (!httpContext.User.Identity.IsAuthenticated && context.Request.Path.StartsWithSegments("/Patient") &&
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

            return services;
        }

        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                ClaimsPrincipal claimUser = context.User;
                if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest" && claimUser.Identity.IsAuthenticated == false && context.Request.Headers["Not-Auth-Required"] != "true")
                {
                    context.Response.StatusCode = 401;
                    return;
                }
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/Patient/Home/Index");
                    return;
                }
                await next();
                if (context.Response.StatusCode == 403 || context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/Account/NotFound");
                    return;
                }
            });

            return app;
        }
    }

}
