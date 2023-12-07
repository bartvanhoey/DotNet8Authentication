using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet8Auth.Shared.Models.Authentication
{
    public class Response
    {
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        
    }

    public class RegisterResponse
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public string? Code { get; set; } 
        public string? UserId { get; set; }
    }
}