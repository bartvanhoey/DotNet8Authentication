using System.Net.Http.Json;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Register;
using DotNet8Auth.Shared.Models.Logging;

namespace DotNet8Auth.BlazorWasmApp.Services.Logging
{
    public interface ISerilogService
    {
        Task<CreateLogEntryResult> CreateLogEntry(CreateLogEntryInputModel input);
    }

    public class CreateLogEntryResult
    {
    }

    public class SerilogService(IHttpClientFactory clientFactory) : ISerilogService
    {
        private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

        public async Task<CreateLogEntryResult> CreateLogEntry(CreateLogEntryInputModel input)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/serilog/create-log-entry", input);
                var result = await response.Content.ReadFromJsonAsync<SerilogResponse>();
            }
            catch (Exception)
            {
                Console.Write("CreateLogEntryResult");
            }

            return new CreateLogEntryResult();
        }
    }
}