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

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                           throw new InvalidOperationException("Connection string not found");
    builder.RegisterSerilog();

    Log.Information("Starting the web host");

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    
    builder.RegisterSwagger();

    // Setup DbContext
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
    var validAudiences = builder.Configuration.GetSection("Jwt:ValidAudiences").Get<List<string>>()
                         ?? throw new InvalidOperationException("'Audience' not found.");

    var validIssuer = builder.Configuration["Jwt:ValidIssuer"]
                      ?? throw new InvalidOperationException("'Issuer' not found.");

    var securityKey = builder.Configuration["Jwt:SecurityKey"]
                      ?? throw new InvalidOperationException("'SecurityKey' not found.");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = AuthenticationScheme;
        options.DefaultChallengeScheme = AuthenticationScheme;
        options.DefaultScheme = AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            // ValidAudience = validAudience,
            ValidAudiences = validAudiences,
            ValidIssuer = validIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(UTF8.GetBytes(securityKey)),
            ClockSkew = new TimeSpan(0, 0, 5)
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = ctx => LogAttempt(ctx.Request.Headers, "OnChallenge: 401 NotAuthorized", Log.Logger),
            OnTokenValidated = ctx => LogAttempt(ctx.Request.Headers, "OnTokenValidated: Authorized", Log.Logger)
        };
    });
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

Task LogAttempt(IHeaderDictionary headers, string eventType, ILogger logger)
{
    var authorizationHeader = headers.Authorization.FirstOrDefault();
    if (authorizationHeader is null)
    {
        Out.WriteLine($"{eventType}. JWT not present");
        logger.Information($"{eventType}. JWT not present");
    }
    else
    {
        var jwtString = authorizationHeader.Substring("Bearer ".Length);
        var jwt = new JwtSecurityToken(jwtString);

        logger.Information(
            $"{eventType}. Expiration: {jwt.ValidTo.ToLongTimeString()}. System time: {DateTime.UtcNow.ToLongTimeString()}");
        Out.WriteLine(
            $"{eventType}. Expiration: {jwt.ValidTo.ToLongTimeString()}. System time: {DateTime.UtcNow.ToLongTimeString()}");
    }

    return CompletedTask;
}