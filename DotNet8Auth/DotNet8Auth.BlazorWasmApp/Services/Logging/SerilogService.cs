using System.Net.Http.Json;
using DotNet8Auth.Shared.Models.Logging;

namespace DotNet8Auth.BlazorWasmApp.Services.Logging;

public class SerilogService(IHttpClientFactory clientFactory) : ISerilogService
{
    private readonly HttpClient _http = clientFactory.CreateClient("ServerAPI");

    private async Task<CreateLogEntryResult> CreateLogEntry(string level, string message)
    {
        try
        {
            var input = new CreateLogEntryInputModel(level, message);
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

    public Task<CreateLogEntryResult> LogWarning(string message) => CreateLogEntry("warning",  message);

    public Task<CreateLogEntryResult> LogError(string message ) => CreateLogEntry("error",  message);
    public Task<CreateLogEntryResult> LogError(Exception exception) 
        => CreateLogEntry("error",   $"{exception.GetType()} - {exception.Message}");

    public Task<CreateLogEntryResult> LogCritical(string message) => CreateLogEntry("critical",  message);

    public Task<CreateLogEntryResult> LogTrace(string message) => CreateLogEntry("trace",  message);

    public Task<CreateLogEntryResult> LogDebug(string message) => CreateLogEntry("debug",  message);
}