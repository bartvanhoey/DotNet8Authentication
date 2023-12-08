using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet8Auth.Shared.Models.Authentication
{
    public class LoginResponse
    {

        public string? AccessToken { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Successful { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string? Status { get; set; }
        public string? TraceId { get; set; }
        public string? Message { get; set; }

    }

    public class RegisterResponse
    {
        public string? Status { get; set; }
        public string? Code { get; set; }
        public string? UserId { get; set; }
        public string? Message { get; set; }

    }

    public class ConfirmEmailResponse
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public string? Code { get; set; }
        public string? UserId { get; set; }
    }
}