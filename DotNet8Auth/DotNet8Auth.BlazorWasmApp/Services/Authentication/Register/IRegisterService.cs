using DotNet8Auth.Shared.Models.Authentication.Register;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Register;

public interface IRegisterService
{
    Task<AuthRegisterResult> RegisterAsync(RegisterInputModel input);
}