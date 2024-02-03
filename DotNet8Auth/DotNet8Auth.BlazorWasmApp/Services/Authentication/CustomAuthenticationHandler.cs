using System.Net;
using System.Net.Http.Headers;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Refresh;
using DotNet8Auth.BlazorWasmApp.Services.Authentication.Token;
using static System.Console;
using static System.String;

namespace DotNet8Auth.BlazorWasmApp.Services.Authentication;

public class CustomAuthenticationHandler(
    IConfiguration configuration,
    IJwtTokenService jwtTokenService,
    IHttpClientFactory clientFactory
)
    : DelegatingHandler //AuthorizationMessageHandler   
{
    private bool _refreshing;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = await jwtTokenService.GetAccessTokenAsync(cancellationToken);
        var isToServer = request.RequestUri?.AbsoluteUri.StartsWith(configuration["ServerUrl"] ?? "") ?? false;

        if (isToServer && !IsNullOrEmpty(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        bool iShouldRefresh;
        HttpResponseMessage? response = null;
        try
        {
            response = await base.SendAsync(request, cancellationToken);
            iShouldRefresh = response.StatusCode == HttpStatusCode.Unauthorized;
            if (iShouldRefresh == false) return response;
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(HttpRequestException))
                WriteLine("----");
            else
                WriteLine(e);
            iShouldRefresh = true;
        }

        if (_refreshing || IsNullOrEmpty(accessToken) || !iShouldRefresh) return response!;

        try
        {
            _refreshing = true;
            var refreshService = new RefreshService(clientFactory, jwtTokenService);
            var refreshResult = await refreshService.RefreshAsync();
            if (!refreshResult.Succeeded) return response!;

            accessToken = await jwtTokenService.GetAccessTokenAsync(cancellationToken);

            if (jwtTokenService.IsAccessTokenValid(accessToken) && isToServer)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
        finally
        {
            _refreshing = false;
        }
    }
}