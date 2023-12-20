using System.Net.Http.Json;
using DotNet8Auth.Shared.Extensions;
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
            await response.Content.ReadFromJsonAsync<SerilogResponse>();
            if (response.IsSuccessStatusCode) return new CreateLogEntryResult();
        }
        catch (Exception)
        {
            Console.Write("CreateLogEntryResult");
        }

        return new CreateLogEntryResult(false);
    }

    public Task<CreateLogEntryResult> LogWarning(string message, string? methodName) => CreateLogEntry("warning",
        methodName.IsNullOrWhiteSpace() ? message : $"{methodName} : {message}");

    public Task<CreateLogEntryResult> LogError(string message, string? methodName) => CreateLogEntry("error",
        methodName.IsNullOrWhiteSpace() ? message : $"{methodName} : {message}");

    public Task<CreateLogEntryResult> LogError(Exception exception, string? methodName)
        => CreateLogEntry("error",
            methodName.IsNullOrWhiteSpace()
                ? $"{exception.GetType()} - {exception.Message}"
                : $"{methodName} : {exception.GetType()} - {exception.Message}");

    public Task<CreateLogEntryResult> LogCritical(string message, string? methodName) => CreateLogEntry("critical",
        methodName.IsNullOrWhiteSpace() ? message : $"{methodName} : {message}");

    public Task<CreateLogEntryResult> LogTrace(string message, string? methodName) => CreateLogEntry("trace",
        methodName.IsNullOrWhiteSpace() ? message : $"{methodName} : {message}");

    public Task<CreateLogEntryResult> LogDebug(string message, string? methodName) => CreateLogEntry("debug",
        methodName.IsNullOrWhiteSpace() ? message : $"{methodName} : {message}");
}