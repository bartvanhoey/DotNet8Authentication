namespace DotNet8Auth.Shared.Models.Authentication.ChangePassword
{
    public class ChangePasswordError(string code, string description)
    {
        public string Code { get; } = code;
        public string Description { get; } = description;
    }
}