using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.Xml;
using DotNet8Auth.API;
using DotNet8Auth.API.Authentication;
using DotNet8Auth.API.Data;
using DotNet8Auth.Shared.Models.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using static System.Console;
using static System.Text.Encoding;
using static System.Threading.Tasks.Task;
using static Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults;

var builder = WebApplication.CreateBuilder(args);

// 
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In= ParameterLocation.Header,
        Description = "Please enter 'Bearer [access-token] in the Value input field'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {{scheme , Array.Empty<string>()}});
});

// Setup DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Setup Identity
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();


builder.Services.SetupEmailClient(builder.Configuration);

// builder.Services.AddScoped<IdentityUserAccessor>();

builder.Services.AddCorsPolicy();

// Adding Authentication
var validAudience = builder.Configuration["Jwt:ValidAudience"];
if(string.IsNullOrWhiteSpace(validAudience)) throw new InvalidOperationException("'Audience' not found.");

var validIssuer = builder.Configuration["Jwt:ValidIssuer"];
if(string.IsNullOrWhiteSpace(validIssuer)) throw new InvalidOperationException("'Issuer' not found.");

var securityKey = builder.Configuration["Jwt:SecurityKey"] ?? throw new InvalidOperationException("'SecurityKey' not found.");

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
        ValidAudience = validAudience,
        ValidIssuer = validIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(UTF8.GetBytes(securityKey)),
        ClockSkew = new TimeSpan(0,0,5)
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = ctx => LogAttempt(ctx.Request.Headers, "OnChallenge: 401 NotAuthorized "),
        OnTokenValidated = ctx => LogAttempt(ctx.Request.Headers, "OnTokenValidated: Authorized")
    };
});

var app = builder.Build();

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

Task LogAttempt(IHeaderDictionary headers, string eventType)
{
    // var logger = loggerFactory.CreateLogger<Program>();

    var authorizationHeader = headers.Authorization.FirstOrDefault();
    if (authorizationHeader is null)
        Out.WriteLine($"{eventType}. JWT not present");
        // logger.LogInformation($"{eventType}. JWT not present");
    else
    {
        var jwtString = authorizationHeader.Substring("Bearer ".Length);
        var jwt = new JwtSecurityToken(jwtString);

        // logger.LogInformation($"{eventType}. Expiration: {jwt.ValidTo.ToLongTimeString()}. System time: {DateTime.UtcNow.ToLongTimeString()}");
        Out.WriteLine($"{eventType}. Expiration: {jwt.ValidTo.ToLongTimeString()}. System time: {DateTime.UtcNow.ToLongTimeString()}");
    }

    return CompletedTask;
}

