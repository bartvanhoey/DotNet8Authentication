using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet8Auth.Shared.Models.Authentication
{
    public class LoginResult
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string OriginalEmail { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool Successful { get; set; }
        public string Error { get; set; }
    }
}