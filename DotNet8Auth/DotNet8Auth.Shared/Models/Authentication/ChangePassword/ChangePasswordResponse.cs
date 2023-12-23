using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet8Auth.Shared.Models.Authentication.ChangePassword;

public class ChangePasswordResponse(string? status)
{
    public ChangePasswordResponse(string? status, string? message) : this(status) => Message = message;
    public ChangePasswordResponse(string? status, IEnumerable<ChangePasswordError> errors) : this(status) => Errors = errors;

    public string? Status { get; set; } = status;
    public string? Message { get; set; }

    public IEnumerable<ChangePasswordError>? Errors { get; set; }

}