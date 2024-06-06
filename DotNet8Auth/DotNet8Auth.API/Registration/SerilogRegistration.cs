using System.Diagnostics;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace DotNet8Auth.API.Registration;

public static class SerilogRegistration
{
    public static void SetupSerilog(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string not found");
        
        var logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.WithProperty("Application", "DotNet8Auth")
            .Enrich.WithProperty("EnvironmentName", builder.Environment)
            .WriteTo.MSSqlServer(
                connectionString: connectionString,
                sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs" })
            .CreateLogger();

        builder.Host.UseSerilog(logger);

        Serilog.Debugging.SelfLog.Enable(msg =>
        {
            Debug.Print(msg);
            Debugger.Break();
        });

    }
}