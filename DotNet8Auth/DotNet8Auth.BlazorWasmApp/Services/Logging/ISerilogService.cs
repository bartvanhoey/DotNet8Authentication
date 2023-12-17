using DotNet8Auth.Shared.Models.Logging;

namespace DotNet8Auth.BlazorWasmApp.Services.Logging;

public interface ISerilogService
{
    Task<CreateLogEntryResult> LogWarning(CreateLogEntryInputModel input);
    Task<CreateLogEntryResult> LogError(CreateLogEntryInputModel input);
    Task<CreateLogEntryResult> LogCritical(CreateLogEntryInputModel input);
    Task<CreateLogEntryResult> LogTrace(CreateLogEntryInputModel input);
    Task<CreateLogEntryResult> LogDebug(CreateLogEntryInputModel input);
        
        
}