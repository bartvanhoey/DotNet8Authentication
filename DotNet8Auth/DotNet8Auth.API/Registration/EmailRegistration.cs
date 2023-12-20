using DotNet8Auth.API.Services.Email;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DotNet8Auth.API.Registration;

public static class EmailSetupRegistration
{
    public static void SetupEmailClient(this WebApplicationBuilder builder)
    {
        // if (builder.Configuration.GetRequiredSection("ASPNETCORE_ENVIRONMENT").Value == "Production")
        //     builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityProductionEmailSender>();
        // else
        //     builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityDevelopmentEmailSender>();

        // TODO Do not forget to setup correctly in production
       builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityProductionEmailSender>();
    }
}