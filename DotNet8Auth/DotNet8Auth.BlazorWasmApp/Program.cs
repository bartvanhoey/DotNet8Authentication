using Blazored.LocalStorage;
using DotNet8Auth.BlazorWasmApp;
using DotNet8Auth.BlazorWasmApp.Services.Authentication;
using DotNet8Auth.BlazorWasmApp.Services.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();


builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();

var serverBaseAddress = builder.Configuration["ServerUrl"] ?? "";
builder.Services.AddTransient<CustomAuthenticationHandler>();
builder.Services.AddHttpClient("ServerAPI",
        client =>
        {
            client.BaseAddress = new Uri(serverBaseAddress);
        })
    .AddHttpMessageHandler<CustomAuthenticationHandler>();

builder.Services.AddScoped<ISerilogService, SerilogService>();
builder.Services.AddAuthenticationServices();

await builder.Build().RunAsync();