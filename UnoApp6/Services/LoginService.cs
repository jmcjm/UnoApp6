using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace UnoApp6.Services;

public class JwtAuthenticationService(HttpClient httpClient, IConfiguration configuration) : IAuthenticationService
{
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    public string[] Providers { get; } = configuration.GetSection("Authentication:Providers").Get<string[]>() ?? [];
    private string? _token;

    public async ValueTask<bool> LoginAsync(
        IDispatcher? dispatcher,
        IDictionary<string, string>? credentials = null,
        string? provider = null,
        CancellationToken? cancellationToken = null)
    {
        if (credentials == null || !credentials.TryGetValue("login", out var login) || string.IsNullOrEmpty(login) ||
            !credentials.TryGetValue("password", out var password) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        try
        {
            var request = new { login, password };
            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(
                "login",
                content,
                cancellationToken ?? CancellationToken.None);

            if (!response.IsSuccessStatusCode)
                return false;

            var responseContent = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent);

            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
                return false;

            _token = loginResponse.Token;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async ValueTask<bool> RefreshAsync(CancellationToken? cancellationToken = null)
    {
        if (string.IsNullOrEmpty(_token))
            return false;

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "refresh");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _httpClient.SendAsync(
                request,
                cancellationToken ?? CancellationToken.None);

            if (!response.IsSuccessStatusCode)
                return false;

            var responseContent = await response.Content.ReadAsStringAsync();
            var refreshResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent);

            if (refreshResponse == null || string.IsNullOrEmpty(refreshResponse.Token))
                return false;

            _token = refreshResponse.Token;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async ValueTask<bool> LogoutAsync(
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        IDispatcher? dispatcher,
        CancellationToken? cancellationToken = null)
    {
        _token = null;
        LoggedOut?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public ValueTask<bool> IsAuthenticated(CancellationToken? cancellationToken = null)
    {
        var isAuthenticated = !string.IsNullOrEmpty(_token);
        return new ValueTask<bool>(isAuthenticated);
    }

    public event EventHandler? LoggedOut;

    private record LoginResponse(string Token);
}
