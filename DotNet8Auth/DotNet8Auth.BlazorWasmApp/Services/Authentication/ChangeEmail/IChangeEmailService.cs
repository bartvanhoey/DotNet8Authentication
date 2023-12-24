using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.ChangeEmail;

public interface IChangeEmailService
{
    Task<AuthIsEmailConfirmedResult> IsEmailConfirmedAsync();
    Task<AuthChangeEmailResult> ChangeEmailAsync(string newEmail);
    Task<AuthConfirmChangeEmailResult> ConfirmChangeEmailAsync(string email, string newEmail, string code);
}