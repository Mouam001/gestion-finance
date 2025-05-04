using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces;
using Common.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Business.Implementations
{
    public class ObpService : IObpService
    {
        private readonly HttpClient _httpClient;
        private string _accessToken;

        private const string ApiUrl = "https://apisandbox.openbankproject.com";
        private const string ConsumerKey = "2hp1wokighzsg1sulh4p3pgvc2dmnp0ymegi45up";
        private const string Username = "katja.fi.29@example.com";
        private const string Password = "ca0317";

        public ObpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken))
                return _accessToken;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization",
                $"DirectLogin username={Username},password={Password},consumer_key={ConsumerKey}");

            var response = await _httpClient.PostAsync($"{ApiUrl}/my/logins/direct", null);
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erreur lors de l'authentification : {responseJson}");

            var responseObject = JObject.Parse(responseJson);
            _accessToken = responseObject["token"]?.ToString();

            if (string.IsNullOrEmpty(_accessToken))
                throw new Exception("Le token d'accès est vide !");

            return _accessToken;
        }

        public async Task<List<BankDto>> GetBanksAsync()
        {
            var response = await _httpClient.GetAsync($"{ApiUrl}/obp/v4.0.0/banks");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erreur lors de la récupération des banques : {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(json);
            return obj["banks"].ToObject<List<BankDto>>();
        }

        
        public async Task<List<BankAccountDetailsDto>> GetUserBanksAsync()
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"DirectLogin token={token}");
            
            var response = await _httpClient.GetAsync($"{ApiUrl}/obp/v5.1.0/banks/rbs/accounts");
            var json = await response.Content.ReadAsStringAsync();
            
            if(!response.IsSuccessStatusCode)
                throw new Exception($"Erreur lors de la récupération des comptes utilisateurs : {json}");
            var accounts = JArray.Parse(json);

            return accounts.Select(acc => new BankAccountDetailsDto
            {
                Id = acc["id"].ToString(),
                Label = acc["label"].ToString(),
                BankId = acc["bank"].ToString(),
                ViewAvailable = acc["views_available"]?.Select(view => new ViewDetailsDto
                {
                    Id = view["id"].ToString(),
                    Shortname = view["short_name"].ToString(),
                    IsPublic = view["is_public"].ToString()
                }).ToList() ?? new List<ViewDetailsDto>()
            }).ToList();
        }

        public async Task<string> CreatAccountAsync(string userId, string bankId)
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("DirectLogin", $"token={token}");

            var accountData = new
            {
                user_id = userId,
                label = "New Account",
                product_code = "PRODUCT_001",
                balance = new { currency = "EUR", amount = 1000 },
                branch_id = "Branch1",
                account_routings = new[] {
                    new { scheme = "OBP", address = bankId }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(accountData), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{ApiUrl}/obp/v4.0.0/banks/{bankId}/accounts", content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erreur lors de la création du compte bancaire : {response.StatusCode}");

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAccountDetailsAsync(string bankId, string accountId)
        {
            var token = await GetAccessTokenAsync();
            var url = $"{ApiUrl}/obp/v5.1.0/banks/{bankId}/accounts/{accountId}/balances";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("DirectLogin", $"token={token}");

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<TransactionDto>> GetTransactionsAsync(string bankId, string accountId)
        {
            var token = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"DirectLogin token={token}");

            var viewId = "owner"; // par défaut, ou à passer en paramètre
            var uri = $"{ApiUrl}/obp/v5.1.0/banks/{bankId}/accounts/{accountId}/{viewId}/transactions";

            var response = await _httpClient.GetAsync(uri);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erreur lors de la récupération des transactions : {json}");

            var transactions = JObject.Parse(json)["transactions"];

            return transactions.Select(t => new TransactionDto
            {
                Id = (string)t["id"],
                Type = (string)t["details"]?["type"],
                Description = (string)t["details"]?["description"],
                Amount = decimal.TryParse(Convert.ToString(t["details"]?["value"]?["amount"]), System.Globalization.NumberStyles.Any,
                         System.Globalization.CultureInfo.InvariantCulture,out var amt) ? amt : 0,
                Currency = (string)t["details"]?["value"]?["currency"],
                PostedDate = DateTime.TryParse((string)t["details"]?["posted"], out var posted) ? posted : DateTime.MinValue,
                CompletedDate = DateTime.TryParse((string)t["details"]?["completed"], out var completed) ? completed : DateTime.MinValue
            }).ToList();
        }

        public async Task<List<AccountDto>> GetAccountsAsync()
        {
            var token = await GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiUrl}/obp/v5.1.0/my/accounts");
            request.Headers.Authorization = new AuthenticationHeaderValue("DirectLogin", $"token={token}");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Erreur lors de la récupération des comptes");

            var json = await response.Content.ReadAsStringAsync();
            var parsed = JObject.Parse(json);

            return parsed["accounts"].Select(acc => new AccountDto
            {
                Id = (string)acc["id"],
                Label = (string)acc["label"],
                BankId = (string)acc["bank_id"],
                Views = acc["views"]?.Select(v => (string)v["id"]).ToList() ?? new List<string>()
            }).ToList();
        }

        public async Task<string> GetMyBanksRawAsync()
        {
            var token = await GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ApiUrl}/obp/v4.0.0/my/banks");
            request.Headers.Authorization = new AuthenticationHeaderValue("DirectLogin", $"token={token}");

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
