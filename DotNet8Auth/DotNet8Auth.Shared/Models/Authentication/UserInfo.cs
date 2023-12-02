using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet8Auth.Shared.Models.Authentication
{
public class UserInfo
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
    }
}