global using static System.String;
using DotNet8Auth.API.Authentication;
using DotNet8Auth.API.Data;
using DotNet8Auth.API.Registration;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
try
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

    builder.RegisterSerilog();
    Log.Information("Starting the web host");

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    
    builder.RegisterSwagger();
    builder.RegisterDatabase();

    builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();

    builder.SetupEmailClient();
    builder.AddCorsPolicy();
    builder.RegisterJwtAuthentication();
    
    Log.Information("Services registered");
}
catch (Exception exception)
{
    Log.Fatal(exception, "host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

var app = builder.Build();

app.UseSerilogRequestLogging(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("CorsPolicy");

// Should this be here? and what does it do exactly? This
// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();

