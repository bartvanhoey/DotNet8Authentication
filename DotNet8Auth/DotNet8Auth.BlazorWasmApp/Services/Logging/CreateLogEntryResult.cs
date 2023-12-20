namespace DotNet8Auth.BlazorWasmApp.Services.Logging;

public class CreateLogEntryResult(bool isSuccessFul)
{
    public CreateLogEntryResult() : this(true)
    {
    }
    public bool IsSuccessFul { get; set; } = isSuccessFul;
}