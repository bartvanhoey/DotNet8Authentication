namespace DotNet8Auth.BlazorWasmApp.Authentication.Register
{
    public class RegisterResult
    {
        public string? Status { get; set; }
        public string? Message { get; set; }

        public bool Succeeded => Message == "Success";
    }
}