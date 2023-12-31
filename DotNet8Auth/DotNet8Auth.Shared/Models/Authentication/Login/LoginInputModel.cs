using System.ComponentModel.DataAnnotations;

namespace DotNet8Auth.Shared.Models.Authentication.Login;

public sealed class LoginInputModel : BaseInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}

public class BaseInputModel
{
}