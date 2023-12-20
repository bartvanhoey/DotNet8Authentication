using DotNet8Auth.Shared.Models.Logging;

namespace DotNet8Auth.BlazorWasmApp.Services.Logging;

public interface ISerilogService
{
    Task<CreateLogEntryResult> LogWarning(string message);
    Task<CreateLogEntryResult> LogError(string message);
    Task<CreateLogEntryResult> LogError(Exception exception);
    Task<CreateLogEntryResult> LogCritical(string message);
    Task<CreateLogEntryResult> LogTrace(string message);
    Task<CreateLogEntryResult> LogDebug(string message);
        
        
}