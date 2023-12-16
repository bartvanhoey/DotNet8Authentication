namespace DotNet8Auth.BlazorWasmApp.Services.Authentication.Refresh
{
    public class RefreshResult
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Successful{ get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; 
        }


    
    }
}