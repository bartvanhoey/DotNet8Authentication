using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet8Auth.Shared.Models.Authentication
{
    public class LoginResult
    {
        public required string AccessToken { get; set; }
        public DateTime ValidTo { get; set; }
    }
}