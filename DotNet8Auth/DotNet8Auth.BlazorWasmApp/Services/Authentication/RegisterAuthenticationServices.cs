﻿using DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangeEmail;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangePassword;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.ConfirmEmail;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.ForgotPassword;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Login;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Logout;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Profile;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Register;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.ResendEmailConfirmation;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.ResetPassword;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Token;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication;

public static class RegisterAuthenticationServices
{
    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IRegisterService, RegisterService>();
        services.AddScoped<IConfirmEmailService, ConfirmEmailService>();
        services.AddScoped<IResendEmailConfirmationService, ResendEmailConfirmationService>();
        services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
        services.AddScoped<IResetPasswordService, ResetPasswordService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<ILogoutService,LogoutService>();
        services.AddScoped<IIdentityAccessor, IdentityAccessor>();
        services.AddScoped<IUserHasPasswordService, UserHasPasswordService>();
        services.AddScoped<IChangePasswordService, ChangePasswordService>();
        services.AddScoped<IChangeEmailService, ChangeEmailService>();
    }
}