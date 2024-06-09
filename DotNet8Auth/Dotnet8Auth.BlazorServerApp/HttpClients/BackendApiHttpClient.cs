// using Dotnet8Auth.BlazorServerApp.Services.Authentication;
// using DotNet8Auth.Shared.Models.Authentication.Login;
//
// namespace Dotnet8Auth.BlazorServerApp.HttpClients;
//
// public class BackendApiHttpClient: IBackendApiHttpClient
// {
//     private readonly HttpClient _httpClient;
//
//     public BackendApiHttpClient(HttpClient httpClient)
//     {
//         _httpClient = httpClient;
//     }
//
//     // public async Task<ApiResponse<string>> RegisterUserAsync(UserRegisterInput model, CancellationToken? cancellationToken = null)
//     // {
//     //     return await ApiResponse<string>.HandleExceptionAsync(async () =>
//     //     {
//     //         var response =
//     //             await _httpClient.PostAsJsonAsync("api/account", model, cancellationToken ?? CancellationToken.None);
//     //
//     //         response.EnsureSuccessStatusCode();
//     //
//     //         return await response.Content.ReadFromJsonAsync<ApiResponse<string>>(cancellationToken ??
//     //             CancellationToken.None);
//     //     });
//     // }
//     public async Task<ApiResponse<AuthResponse>> LoginUserAsync(LoginInputModel model, CancellationToken? cancellationToken = null)
//     {
//         return await ApiResponse<AuthResponse>.HandleExceptionAsync(async () =>
//         {
//             var response = await _httpClient.PostAsJsonAsync("api/account/login", model);
//
//             response.EnsureSuccessStatusCode();
//             
//             
//             var result = await response.Content.ReadFromJsonAsync<LoginResult>();
//
//             return await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>(cancellationToken ?? CancellationToken.None);
//         });
//     }
//     public async Task<ApiResponse<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken? cancellationToken = null)
//     {
//         return await ApiResponse<AuthResponse>.HandleExceptionAsync(async () =>
//         {
//             var response = await _httpClient.PostAsJsonAsync("api/account/refresh", new{ refreshToken });
//
//             response.EnsureSuccessStatusCode();
//
//             return await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponse>>(cancellationToken ?? CancellationToken.None);
//         });
//     }
// }