using System.Net.Http.Json;
using DotNet8Auth.Shared.Models.Logging;

namespace DotNet8Auth.BlazorWasmApp.Services.Logging
{
    public interface ISerilogService
    {
        // Task<CreateLogEntryResult> CreateLogEntry(CreateLogEntryInputModel input);
        Task<CreateLogEntryResult> LogWarning(CreateLogEntryInputModel input);
        Task<CreateLogEntryResult> LogError(CreateLogEntryInputModel input);
        Task<CreateLogEntryResult> LogCritical(CreateLogEntryInputModel input);
        Task<CreateLogEntryResult> LogTrace(CreateLogEntryInputModel input);
        Task<CreateLogEntryResult> LogDebug(CreateLogEntryInputModel input);
        
        
    }

    public class CreateLogEntryResult
    {
    }

    public class SerilogService(IHttpClientFactory clientFactory) : ISerilogService
    {
        private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

        private async Task<CreateLogEntryResult> CreateLogEntry(CreateLogEntryInputModel input)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/serilog/create-log-entry", input);
                var result = await response.Content.ReadFromJsonAsync<SerilogResponse>();
                return new CreateLogEntryResult();    
            }
            catch (Exception)
            {
                Console.Write("CreateLogEntryResult");
            }

            return new CreateLogEntryResult();
        }

        public async Task<CreateLogEntryResult> LogWarning(CreateLogEntryInputModel input)
        {
            input.Level = "warning";
            var createLogEntryResult = await CreateLogEntry(input);
            return createLogEntryResult;
        }

        public async Task<CreateLogEntryResult> LogError(CreateLogEntryInputModel input)
        {
            input.Level = "error";
            var createLogEntryResult = await CreateLogEntry(input);
            return createLogEntryResult;
        }

        public async Task<CreateLogEntryResult> LogCritical(CreateLogEntryInputModel input)
        {
            input.Level = "critical";
            var createLogEntryResult = await CreateLogEntry(input);
            return createLogEntryResult;
        }

        public async Task<CreateLogEntryResult> LogTrace(CreateLogEntryInputModel input)
        {
            input.Level = "trace";
            var createLogEntryResult = await CreateLogEntry(input);
            return createLogEntryResult;
        }

        public async Task<CreateLogEntryResult> LogDebug(CreateLogEntryInputModel input)
        {
            input.Level = "debug";
            var createLogEntryResult = await CreateLogEntry(input);
            return createLogEntryResult;
        }
    }
}