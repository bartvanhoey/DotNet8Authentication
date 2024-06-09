using System.Reflection;
using Dotnet8Auth.BlazorServerApp;
using Dotnet8Auth.BlazorServerApp.Components;
using Dotnet8Auth.BlazorServerApp.HttpClients;
using Dotnet8Auth.BlazorServerApp.Services.Authentication;
using Dotnet8Auth.BlazorServerApp.Services.Authentication.Login;
using Dotnet8Auth.BlazorServerApp.Services.Authentication.Token;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorizationCore();


builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

var serverBaseAddress = builder.Configuration["ServerUrl"] ?? "";
// builder.Services.AddHttpClient<IBackendApiHttpClient, BackendApiHttpClient>(options =>
// {
//     // options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Urls:BackendApi"));
//     options.BaseAddress = new Uri(serverBaseAddress);
//     options.Timeout = TimeSpan.FromSeconds(30);
//     // options.DefaultRequestHeaders.Accept.Add();
//     // options.DefaultRequestHeaders.TryAddWithoutValidation("Service", Assembly.GetAssembly(typeof(Program))?.GetName().Name);
// });

builder.Services.AddTransient<CustomAuthenticationHandler>();
builder.Services.AddHttpClient("ServerAPI",
        client =>
        {
            client.BaseAddress = new Uri(serverBaseAddress);
        })
    .AddHttpMessageHandler<CustomAuthenticationHandler>();

builder.Services.AddAuthenticationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles( );
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
