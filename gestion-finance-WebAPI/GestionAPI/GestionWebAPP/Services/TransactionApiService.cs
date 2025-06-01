using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Common.DTO;
using Common.Requests;

public class TransactionApiService
{
    private readonly HttpClient _http;

    public TransactionApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<TransactionResult> CreateTransactionAsync(CreateTransactionRequest request, string jwtToken)
    {
        // Ajout du token dans l'en-tête
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        var jsonRequest = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync("api/transaction", content);

        if (response.IsSuccessStatusCode)
        {
            return new TransactionResult { Success = true };
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        return new TransactionResult
        {
            Success = false,
            ErrorMessage = $"Erreur {response.StatusCode}: {errorContent}"
        };
    }

    public async Task<List<TransactionDto>> GetUserTransactionsAsync(string jwtToken)
    {
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        var response = await _http.GetAsync("api/transaction");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Réponse JSON brute :");
            Console.WriteLine(jsonResponse);

            return JsonSerializer.Deserialize<List<TransactionDto>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<TransactionDto>();
        }

        Console.WriteLine($"Échec du chargement des transactions : {response.StatusCode}");
        return new List<TransactionDto>();
    }
}

public class TransactionResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
}
