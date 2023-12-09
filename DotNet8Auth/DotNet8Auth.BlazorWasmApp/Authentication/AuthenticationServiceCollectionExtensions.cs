﻿using DotNet8Auth.BlazorWasmApp.Authentication.ConfirmEmail;
using DotNet8Auth.BlazorWasmApp.Authentication.Login;
using DotNet8Auth.BlazorWasmApp.Authentication.Register;
using DotNet8Auth.BlazorWasmApp.Authentication.ResendEmailConfirmation;

namespace DotNet8Auth.BlazorWasmApp.Authentication
{
    public static class AuthenticationServiceCollectionExtensions
    {
        
        public static void AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IConfirmEmailService, ConfirmEmailService>();
            services.AddScoped<IResendEmailConfirmationService,ResendEmailConfirmationService>();
        }
    }
}