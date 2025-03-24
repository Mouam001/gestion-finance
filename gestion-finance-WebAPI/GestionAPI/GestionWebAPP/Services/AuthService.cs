using Blazored.LocalStorage;
using Common.Requests;

namespace GestionWebAPP.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly CustomAuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage, CustomAuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> Register(RegisterRequest model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Login(LoginRequest model)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var result = await response.Content.ReadFromJsonAsync<AuthenticateResponse>();
        if (result != null)
        {
            await _localStorage.SetItemAsync("authToken", result.Token);
            _authStateProvider.MarkUserAsAuthenticated(result.Token);
            return true;
        }

        return false;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _authStateProvider.MarkUserAsLoggedOut();
    }
}