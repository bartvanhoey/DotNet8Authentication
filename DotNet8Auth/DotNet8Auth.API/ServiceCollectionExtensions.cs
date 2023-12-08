using DotNet8Auth.API.Services.Email;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DotNet8Auth.API
{
    public static class ServiceCollectionExtensions
    {

        public static void AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }



        public static void SetupEmailClient(this IServiceCollection services, IConfiguration configuration)
        {
            // var environment = configuration.GetRequiredSection("ASPNETCORE_ENVIRONMENT").Value;
            // if (environment == "Production")
            //     services.AddSingleton<IEmailSender<ApplicationUser>, IdentityProductionEmailSender>();
            // else
            //     services.AddSingleton<IEmailSender<ApplicationUser>, IdentityDevelopmentEmailSender>();
            
            services.AddSingleton<IEmailSender<ApplicationUser>, IdentityProductionEmailSender>();

        }



    }
}