using Blazored.LocalStorage;
using Common.Requests;
using System.Text.Json;

using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

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

    public async Task<RegisterResult> Register(RegisterRequest model)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);

            if (response.IsSuccessStatusCode)
            {
                return new RegisterResult { IsSuccess = true };
            }

            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest &&
                response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var validationErrors = JsonSerializer.Deserialize<ApiValidationErrorResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                var allErrors = validationErrors?.Errors
                    .SelectMany(e => e.Value)
                    .ToArray();

                return new RegisterResult
                {
                    IsSuccess = false,
                    ErrorMessage = allErrors is not null ? string.Join("\n", allErrors) : "Validation error"
                };
            }

            return new RegisterResult
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: {content}"
            };
        }
        catch (Exception ex)
        {
            return new RegisterResult
            {
                IsSuccess = false,
                ErrorMessage = $"Exception: {ex.Message}"
            };
        }
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