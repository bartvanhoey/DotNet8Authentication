using System.Net.Http.Json;
using DotNet8Auth.Shared.Models.Logging;

namespace DotNet8Auth.BlazorWasmApp.Services.Logging;

public class SerilogService(IHttpClientFactory clientFactory) : ISerilogService
{
    private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

    private async Task<CreateLogEntryResult> CreateLogEntry(CreateLogEntryInputModel input, string level)
    {
        try
        {
            input.Level = level;
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

    public Task<CreateLogEntryResult> LogWarning(CreateLogEntryInputModel input) => CreateLogEntry(input, "warning");

    public Task<CreateLogEntryResult> LogError(CreateLogEntryInputModel input) => CreateLogEntry(input, "error");

    public Task<CreateLogEntryResult> LogCritical(CreateLogEntryInputModel input) => CreateLogEntry(input, "critical");

    public Task<CreateLogEntryResult> LogTrace(CreateLogEntryInputModel input) => CreateLogEntry(input, "trace");

    public Task<CreateLogEntryResult> LogDebug(CreateLogEntryInputModel input) => CreateLogEntry(input, "debug");
}