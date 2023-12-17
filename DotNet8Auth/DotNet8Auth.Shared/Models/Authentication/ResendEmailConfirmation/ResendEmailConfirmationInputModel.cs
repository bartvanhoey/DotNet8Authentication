using System.ComponentModel.DataAnnotations;

namespace DotNet8Auth.Shared.Models.Authentication.ResendEmailConfirmation;

public class ResendEmailConfirmationInputModel
{
    [Required] [EmailAddress] public string Email { get; set; } = "";

    // [Required]
    // [Url]
    // public string CallbackUrl { get; set; } = "";
}