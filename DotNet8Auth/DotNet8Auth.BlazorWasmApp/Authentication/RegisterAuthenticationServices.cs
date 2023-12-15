﻿using DotNet8Auth.BlazorWasmApp.Authentication.ConfirmEmail;
using DotNet8Auth.BlazorWasmApp.Authentication.ForgotPassword;
using DotNet8Auth.BlazorWasmApp.Authentication.Login;
using DotNet8Auth.BlazorWasmApp.Authentication.Logout;
using DotNet8Auth.BlazorWasmApp.Authentication.Profile;
using DotNet8Auth.BlazorWasmApp.Authentication.Refresh;
using DotNet8Auth.BlazorWasmApp.Authentication.Register;
using DotNet8Auth.BlazorWasmApp.Authentication.ResendEmailConfirmation;
using DotNet8Auth.BlazorWasmApp.Authentication.ResetPassword;

namespace DotNet8Auth.BlazorWasmApp.Authentication
{
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
        }
    }
}