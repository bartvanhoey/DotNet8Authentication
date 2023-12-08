using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace DotNet8Auth.API.Services.Email
{
    public class IdentityProductionEmailSender : IEmailSender<ApplicationUser>
    {
        private readonly string googleEmail = string.Empty;
        private readonly string googleAppPassword = string.Empty;

        private readonly IEmailSender emailSender;

        public IdentityProductionEmailSender(IConfiguration configuration)
        {
            googleEmail = configuration.GetRequiredSection("GoogleSmtp:GoogleEmail").Value ?? throw new ArgumentNullException();
            googleAppPassword = configuration.GetRequiredSection("GoogleSmtp:GoogleAppPassword").Value ?? throw new ArgumentNullException();
            emailSender = new ProductionEmailSender(googleEmail, googleAppPassword);
        }



        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
            emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
            emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
            emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");
    }

}