global using static System.String;
using System.IdentityModel.Tokens.Jwt;
using DotNet8Auth.API.Authentication;
using DotNet8Auth.API.Data;
using DotNet8Auth.API.Registration;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using static System.Console;
using static System.Text.Encoding;
using static System.Threading.Tasks.Task;
using static Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;
using ILogger = Serilog.ILogger;
// ReSharper disable TemplateIsNotCompileTimeConstantProblem


var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

    
    builder.RegisterSerilog();

    Log.Information("Starting the web host");

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    
    builder.RegisterSwagger();

    // Setup DbContext
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                           throw new InvalidOperationException("Connection string not found");
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

    // Setup Identity
    builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();

    builder.Services.SetupEmailClient(builder.Configuration);

    builder.Services.AddCorsPolicy();

    // Adding Authentication
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

// app.UseSerilogRequestLogging(); Logs all http requests

// Configure the HTTP request pipeline.
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

