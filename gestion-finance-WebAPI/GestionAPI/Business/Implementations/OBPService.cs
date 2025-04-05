using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Business.Implementations
{
    public class ObpService
    {
        private readonly HttpClient _httpClient;
        private string _accessToken;

        private const string ApiUrl = "https://apisandbox.openbankproject.com";
        private const string ConsumerKey = "2hp1wokighzsg1sulh4p3pgvc2dmnp0ymegi45up"; // Remplace avec ta clé API
        private const string Username = "timo.fi.29@example.com";
        private const string Password = "6addcd";

        public ObpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken))
                return _accessToken;

            // 🔹 Ajouter DirectLogin dans l'en-tête (au lieu du body)
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", 
                $"DirectLogin username={Username},password={Password},consumer_key={ConsumerKey}");

            // 🔹 Envoyer la requête d'authentification
            var response = await _httpClient.PostAsync($"{ApiUrl}/my/logins/direct", null);
            string responseJson = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Réponse API Auth JSON: {responseJson}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur lors de l'authentification : {responseJson}");
            }

            // 🔹 Extraire et retourner le token
            var responseObject = JObject.Parse(responseJson);
            _accessToken = responseObject["token"]?.ToString();

            if (string.IsNullOrEmpty(_accessToken))
            {
                throw new Exception("Le token d'accès est vide !");
            }

            return _accessToken;
        }
        
        public async Task<List<Bank>> GetBanksAsync()
        {
            var response = await _httpClient.GetAsync("https://apisandbox.openbankproject.com/obp/v5.1.0/banks");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur lors de la récupération des banques : {response.StatusCode} - {response.ReasonPhrase}");
            }

            string responseJson = await response.Content.ReadAsStringAsync();

            // Désérialisation de la réponse JSON sans bank_routings
            var banksResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<BankListResponse>(responseJson);
            return banksResponse.Banks;
        }
        
        public async Task<string> GetUserAccountsAsync(string bankId)
        {
            string token = await GetAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"DirectLogin token={token}");

            var response = await _httpClient.GetAsync($"{ApiUrl}/obp/v5.1.0/banks/{bankId}/accounts");
            string responseJson = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Réponse API Get User Accounts JSON: {responseJson}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur lors de la récupération des comptes de l'utilisateur : {responseJson}");
            }

            return responseJson;
        }
        
        public async Task<List<BankAccountDetails>> GetUserBanksAsync(string bankId)
        {
            string accountsJson = await GetUserAccountsAsync(bankId);

            // Vérifier si la réponse est un tableau (array) ou un objet (lorsque vous travaillez avec une liste)
            JArray accountsArray = JArray.Parse(accountsJson);  // Utiliser JArray pour gérer les tableaux JSON

            // 🔹 Extraire les informations sur les comptes bancaires
            var bankAccounts = accountsArray
                .Select(acc => new BankAccountDetails
                {
                    Id = acc["id"]?.ToString(),
                    Label = acc["label"]?.ToString(),
                    BankId = acc["bank_id"]?.ToString(),
                    ViewsAvailable = acc["views_available"]?.Select(view => new ViewDetails
                    {
                        Id = view["id"]?.ToString(),
                        ShortName = view["short_name"]?.ToString(),
                        IsPublic = view["is_public"]?.ToObject<bool>() ?? false
                    }).ToList() ?? new List<ViewDetails>()
                })
                .ToList();

            Console.WriteLine($"Banques associées aux comptes de l'utilisateur: {string.Join(", ", bankAccounts.Select(acc => acc.BankId))}");

            return bankAccounts;
        }
        
        public async Task<string> CreateAccountAsync(string userId, string bankId, string accountType, double initialBalance)
        {
            // Assurez-vous que vous utilisez l'URL correcte pour OBP
            var apiUrl = "https://apisandbox.openbankproject.com"; // URL de l'API OBP Sandbox

            var accountData = new
            {
                user_id = userId,
                label = $"{accountType} Account",
                product_code = $"{bankId}_{accountType}",
                balance = new { currency = "EUR", amount = initialBalance },
                branch_id = "Branch1", // Ajuste selon ta configuration
                account_routings = new[]
                {
                    new { scheme = "OBP", address = bankId }
                }
            };

            var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(accountData);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{apiUrl}/obp/v4.0.0/banks/{bankId}/accounts", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur lors de la création du compte bancaire : {response.StatusCode} - {response.ReasonPhrase}");
            }

            return await response.Content.ReadAsStringAsync();
        }
        
        public async Task<string> GetAccountDetailsAsync(string bankId, string accountId)
        {
            string token = await GetAccessTokenAsync();
            var url = $"https://apisandbox.openbankproject.com/obp/v5.1.0/banks/{bankId}/accounts/{accountId}/balances";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", $"DirectLogin token={token}");
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

      
        
        public async Task<string> GetmyBanksAsync()
        {
            string token = await GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://apisandbox.openbankproject.com/obp/v4.0.0/my/banks");
            request.Headers.Add("Authorization", $"DirectLogin token={token}");
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
        
        public async Task<List<Transaction>> GetTransactionsAsync(string bankId, string accountId, string viewId)
        {
            string token = await GetAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"DirectLogin token={token}");

            var uri = $"{ApiUrl}/obp/v5.1.0/banks/{bankId}/accounts/{accountId}/{viewId}/transactions";
            var response = await _httpClient.GetAsync(uri);
            string responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error retrieving transactions: {responseJson}");

            var transactionsData = JsonConvert.DeserializeObject<TransactionResponse>(responseJson);

            return transactionsData?.Transactions?.Select(t => new Transaction
            {
                Id = t.Id,
                Type = t.Details?.Type,
                Description = t.Details?.Description,
                Amount = t.Details?.Value?.Amount ?? 0,
                Currency = t.Details?.Value?.Currency,
                PostedDate = t.Details?.PostedDate ?? DateTime.MinValue,
                CompletedDate = t.Details?.CompletedDate ?? DateTime.MinValue
            }).ToList() ?? new List<Transaction>();
        }
        
        public async Task<List<Account>> GetAccountsAsync()
        {
            string token = await GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiUrl}/obp/v5.1.0/my/accounts");
            request.Headers.Authorization = new AuthenticationHeaderValue("DirectLogin", $"token={token}");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error retrieving accounts: {error}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            var accounts = json["accounts"]
                .Select(a => new Account
                {
                    Id = (string)a["id"],
                    Label = (string)a["label"],
                    BankId = (string)a["bank_id"],
                    Views = a["views"].Select(v => (string)v["id"]).ToList()
                }).ToList();

            return accounts;
        }

        
        public class BankListResponse
        {
            [JsonProperty("banks")]
            public List<Bank> Banks { get; set; }
        }

        public class Bank
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("short_name")]
            public string ShortName { get; set; }

            [JsonProperty("full_name")]
            public string FullName { get; set; }

            [JsonProperty("logo")]
            public string Logo { get; set; }

            [JsonProperty("website")]
            public string Website { get; set; }
        }
        
        public class BankAccountDetails
        {
            public string Id { get; set; }
            public string Label { get; set; }
            public string BankId { get; set; }
            public List<ViewDetails> ViewsAvailable { get; set; }
        }

// Classe pour stocker les informations sur les vues disponibles
        public class ViewDetails
        {
            public string Id { get; set; }
            public string ShortName { get; set; }
            public bool IsPublic { get; set; }
        }
       
        
        public class Transaction
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public string Currency { get; set; }
            public DateTime PostedDate { get; set; }
            public DateTime CompletedDate { get; set; }
        }
        
        public class TransactionResponse
        {
            [JsonProperty("transactions")]
            public List<TransactionJson> Transactions { get; set; }
        }

        public class TransactionJson
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("details")]
            public TransactionDetailsJson Details { get; set; }
        }

        public class TransactionDetailsJson
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("value")]
            public AmountJson Value { get; set; }

            [JsonProperty("posted")]
            public DateTime PostedDate { get; set; }

            [JsonProperty("completed")]
            public DateTime CompletedDate { get; set; }
        }

        public class AmountJson
        {
            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("amount")]
            public decimal Amount { get; set; }
        }
        
        public class Account
        {
            public string Id { get; set; }
            public string Label { get; set; }
            public string BankId { get; set; }
            public List<string> Views { get; set; }
        }



    }
}


