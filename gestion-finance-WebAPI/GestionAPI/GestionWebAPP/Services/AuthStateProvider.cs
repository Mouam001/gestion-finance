using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace GestionWebAPP.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public AuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            var identity = new ClaimsIdentity();
            if (!string.IsNullOrEmpty(token) && token.Contains('.'))
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                catch (Exception)
                {
                   
                }
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var parts = jwt.Split('.');
            if (parts.Length < 2)
                return Enumerable.Empty<Claim>();

            var payload = parts[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())) ?? Enumerable.Empty<Claim>();
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            // Convertir base64url en base64
            base64 = base64.Replace('-', '+').Replace('_', '/');

            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<string> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }
    }
}
