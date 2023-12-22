using DotNet8Auth.BlazorWasmApp.Services.Authentication.ConfirmEmail;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.ForgotPassword;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Login;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Logout;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Profile;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Register;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.ResendEmailConfirmation;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.ResetPassword;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication;

public static class RegisterAuthenticationServices
{
    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IRegisterService, RegisterService>();
        services.AddScoped<IConfirmEmailService, ConfirmEmailService>();
        services.AddScoped<IResendEmailConfirmationService, ResendEmailConfirmationService>();
        services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
        services.AddScoped<IResetPasswordService, ResetPasswordService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<ILogoutService,LogoutService>();
        services.AddScoped<IIdentityAccessor, IdentityAccessor>();
    }
}