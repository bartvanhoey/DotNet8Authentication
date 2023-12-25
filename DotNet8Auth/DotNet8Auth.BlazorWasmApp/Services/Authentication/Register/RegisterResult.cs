using DotNet8Auth.Shared.Models.Authentication;
using DotNet8Auth.Shared.Models.Authentication.ChangeEmail;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Register;

public class RegisterResult
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public bool Succeeded => Status == "Success";

    public IEnumerable<ControllerResponseError>? Errors { get; set; }

}