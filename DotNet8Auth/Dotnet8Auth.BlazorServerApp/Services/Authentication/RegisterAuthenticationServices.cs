using Dotnet8Auth.BlazorServerApp.Services.Authentication.Login;
using Dotnet8Auth.BlazorServerApp.Services.Authentication.Logout;
using Dotnet8Auth.BlazorServerApp.Services.Authentication.Token;

namespace Dotnet8Auth.BlazorServerApp.Services.Authentication;

public static class RegisterAuthenticationServices
{
    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ILoginService, LoginService>();
        // services.AddScoped<IRegisterService, RegisterService>();
        // services.AddScoped<IConfirmEmailService, ConfirmEmailService>();
        // services.AddScoped<IResendEmailConfirmationService, ResendEmailConfirmationService>();
        // services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
        // services.AddScoped<IResetPasswordService, ResetPasswordService>();
        // services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<ILogoutService,LogoutService>();
        // services.AddScoped<IIdentityAccessor, IdentityAccessor>();
        // services.AddScoped<IUserHasPasswordService, UserHasPasswordService>();
        // services.AddScoped<IChangePasswordService, ChangePasswordService>();
        // services.AddScoped<IChangeEmailService, ChangeEmailService>();
    }
}