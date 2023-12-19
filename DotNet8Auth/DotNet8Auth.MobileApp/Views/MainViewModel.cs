using System.Text;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;
using DotNet8Auth.Shared.Models.Authentication.Login;
using DotNet8Auth.Shared.Models.Authentication.Register;


namespace DotNet8Auth.MobileApp.Views;

public partial class MainViewModel : BaseViewModel
{
    private const string BaseUrl = "https://a2f6-2a02-810d-af40-18b0-28c6-c98c-164f-e4b0.ngrok-free.app";

    private readonly HttpClient _httpClient;

    private string _registerPassword,
        _registerEmail,
        _loginEmail,
        _loginPassword,
        _weatherForecastMessage,
        _registerUserMessage,
        _loginUserMessage;


    public MainViewModel()
    {
        _httpClient = GetHttpClient();
    }

    [RelayCommand]
    private async Task RegisterUserAsync()
    {
        var registerUrl = $"{BaseUrl}/api/account/register";

        var registerModel = new RegisterInputModel
        {
            Email = RegisterEmail,
            Password = RegisterPassword,
            ConfirmPassword = RegisterPassword,
        };

        var json = JsonSerializer.Serialize(registerModel, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(registerUrl, stringContent);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            RegisterUserMessage = "User successfully registered!";
        }
        else
        {
            RegisterUserMessage = $"User registration went wrong - Status code: {response.StatusCode}";
        }

        RegisterPassword = null;
        RegisterEmail = null;
    }


    [RelayCommand]
    private async Task LoginUserAsync()
    {
        var loginUrl = $"{BaseUrl}/api/account/login";

        var loginModel = new LoginInputModel
        {
            Email = LoginEmail,
            Password = LoginPassword,
        };

        var json = JsonSerializer.Serialize(loginModel, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(loginUrl, stringContent);
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            var authenticationResponse = JsonSerializer.Deserialize<LoginResult>(responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            LoginUserMessage = $"User successfully logged in!";
        }
        else
        {
            LoginUserMessage = $"User registration went wrong - Status code: {response.StatusCode}";
        }

        LoginEmail = null;
        LoginPassword = null;
    }

    [RelayCommand]
    private async Task GetWeatherForecastAsync()
    {
        // var weatherForecastUrl = $"{BaseUrl}/api/weatherforecast";
        //
        // if (!string.IsNullOrWhiteSpace(_authenticationResponse?.Token))
        // {
        //     _httpClient.SetBearerToken(_authenticationResponse.Token);
        // }
        //
        // var response = await _httpClient.GetAsync(weatherForecastUrl);
        //
        // if (response.IsSuccessStatusCode)
        // {
        //     var responseContent = await response.Content.ReadAsStringAsync();
        //
        //     var forecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(responseContent, new JsonSerializerOptions
        //     {
        //         PropertyNameCaseInsensitive = true
        //     });
        //
        //     WeatherForecastMessage =  $"{forecasts.FirstOrDefault().TemperatureC} - {forecasts.FirstOrDefault().Summary}";
        // }
        // else
        // {
        //     WeatherForecastMessage = response.StatusCode.ToString();
        // }
    }


    private HttpClient GetHttpClient()
    {
        var httpClient = new HttpClient(GetHttpClientHandler());
        httpClient.DefaultRequestHeaders.Add("Origin", new[] { "IAmAValidAudience" });
        return httpClient;
    }

    private HttpClientHandler GetHttpClientHandler()
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // EXCEPTION : Javax.Net.Ssl.SSLHandshakeException: 'java.security.cert.CertPathValidatorException: Trust anchor for certification path not found.'
        // SOLUTION :
        var httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };
        return httpClientHandler;
    }

    public string WeatherForecastMessage
    {
        get => _weatherForecastMessage;
        set => SetProperty(ref _weatherForecastMessage, value);
    }

    public string RegisterUserMessage
    {
        get => _registerUserMessage;
        set => SetProperty(ref _registerUserMessage, value);
    }

    public string LoginUserMessage
    {
        get => _loginUserMessage;
        set => SetProperty(ref _loginUserMessage, value);
    }


    public string RegisterPassword
    {
        get => _registerPassword;
        set => SetProperty(ref _registerPassword, value);
    }

    public string RegisterEmail
    {
        get => _registerEmail;
        set => SetProperty(ref _registerEmail, value);
    }

    public string LoginEmail
    {
        get => _loginEmail;
        set => SetProperty(ref _loginEmail, value);
    }

    public string LoginPassword
    {
        get => _loginPassword;
        set => SetProperty(ref _loginPassword, value);
    }
}