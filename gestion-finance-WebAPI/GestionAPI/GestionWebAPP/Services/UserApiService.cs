using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Common.DTO;

public class UserApiService
{
    private readonly HttpClient _http;

    public UserApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<UserDto?> GetCurrentUserAsync(string jwtToken)
    {
        
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        var response = await _http.GetAsync("api/user/me");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        // En cas d’erreur, log optionnel
        var error = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Erreur lors de la récupération de l'utilisateur : {response.StatusCode} - {error}");

        return null;
    }
}