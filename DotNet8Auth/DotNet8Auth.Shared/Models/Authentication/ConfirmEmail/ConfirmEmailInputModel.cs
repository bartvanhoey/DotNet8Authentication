namespace DotNet8Auth.Shared.Models.Authentication.ConfirmEmail
{
    public class ConfirmEmailInputModel
    {
        public string UserId { get; set; } = "";
        public string Code { get; set; } = "";
    }
}