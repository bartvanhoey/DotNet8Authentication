using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet8Auth.BlazorWasmApp.Authentication.ResetPassword
{
    public class ResetPasswordResult
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public bool Succeeded => Status == "Success";
    }
}