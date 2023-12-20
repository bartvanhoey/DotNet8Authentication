using DotNet8Auth.API.Services.Email;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DotNet8Auth.API.Registration;

public static class EmailSetupRegistration
{
    public static void SetupEmailClient(this IServiceCollection services, IConfiguration configuration)
    {
        // var environment = configuration.GetRequiredSection("ASPNETCORE_ENVIRONMENT").Value;
        // if (environment == "Production")
        //     services.AddSingleton<IEmailSender<ApplicationUser>, IdentityProductionEmailSender>();
        // else
        //     services.AddSingleton<IEmailSender<ApplicationUser>, IdentityDevelopmentEmailSender>();

        // TODO Do not forget to setup correctly in production
        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityProductionEmailSender>();
    }
}