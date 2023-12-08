using System.ComponentModel.DataAnnotations;

namespace DotNet8Auth.Shared.Models.Authentication
{
    public class ConfirmEmailInputModel
    {
        public string UserId { get; set; } = "";
        public string Code { get; set; } = "";
    }
}