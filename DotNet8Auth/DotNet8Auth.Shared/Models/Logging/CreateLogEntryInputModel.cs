namespace DotNet8Auth.Shared.Models.Logging;

public class CreateLogEntryInputModel
{
    public string? Message { get; set; }
    public string? Level { get; set; }
    public Exception? Exception { get; set; }
}