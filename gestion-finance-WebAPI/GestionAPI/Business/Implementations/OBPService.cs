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
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Business.Implementations
{
    public class ObpService : IObpService
    {
        private readonly HttpClient _httpClient;

        //  private string _accessToken;
        private readonly string _apiUrl;
        private readonly string _consumerKey;

        // private const string Username = "katja.fi.29@example.com";
        // private const string Password = "ca0317";

        public ObpService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _apiUrl = config["obp:ApiUrl"] ??  "https://apisandbox.openbankproject.com";
            _consumerKey = config["obp:consumerKey"] ?? "2hp1wokighzsg1sulh4p3pgvc2dmnp0ymegi45up";

            Console.WriteLine($"ApiUrl: {_apiUrl}");
            Console.WriteLine($"ConsumerKey: {_consumerKey}");
        }
        
        public async Task<string> LoginWithCredentialsAsync(string usernameopb, string passwordopb)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization",
                $"DirectLogin username={usernameopb},password={passwordopb},consumer_key={_consumerKey}");

            var response = await _httpClient.PostAsync($"{_apiUrl}/my/logins/direct", null);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erreur lors de l'authentification OPB");

            var token = JObject.Parse(json)["token"]?.ToString();
            if (string.IsNullOrEmpty(token))
                throw new Exception("Token OPB manquant");

            return token;
        }

        public async Task<List<BankDto>> GetBanksAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiUrl}/obp/v4.0.0/banks");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erreur lors de la récupération des banques : {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(json);
            return obj["banks"].ToObject<List<BankDto>>();
        }


        public async Task<List<BankAccountDetailsDto>> GetUserBanksAsync(string obpToken)
        {

            setTokenHeader(obpToken);
            var response = await _httpClient.GetAsync($"{_apiUrl}/obp/v5.1.0/banks/rbs/accounts");
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
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

        public async Task<string> CreatAccountAsync(string obpToken, string userId, string bankId)
        {
            setTokenHeader(obpToken);

            var accountData = new
            {
                user_id = userId,
                label = "New Account",
                product_code = "PRODUCT_001",
                balance = new { currency = "EUR", amount = 10000 },
                branch_id = "Branch1",
                account_routings = new[]
                {
                    new { scheme = "OBP", address = bankId }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(accountData), Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/obp/v4.0.0/banks/{bankId}/accounts", content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erreur lors de la création du compte bancaire : {response.StatusCode}");

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAccountDetailsAsync(string obpToken, string bankId, string accountId)
        {


            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{_apiUrl}/obp/v5.1.0/banks/{bankId}/accounts/{accountId}/balances");
            request.Headers.Authorization = new AuthenticationHeaderValue("DirectLogin", $"token={obpToken}");

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<TransactionDto>> GetTransactionsAsync(string obpToken, string bankId, string accountId)
        {

            setTokenHeader(obpToken);

            var viewId = "owner"; // par défaut, ou à passer en paramètre
            var uri = $"{_apiUrl}/obp/v5.1.0/banks/{bankId}/accounts/{accountId}/{viewId}/transactions";

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
                Amount = decimal.TryParse(Convert.ToString(t["details"]?["value"]?["amount"]),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out var amt)
                    ? amt
                    : 0,
                Currency = (string)t["details"]?["value"]?["currency"],
                PostedDate = DateTime.TryParse((string)t["details"]?["posted"], out var posted)
                    ? posted
                    : DateTime.MinValue,
                CompletedDate = DateTime.TryParse((string)t["details"]?["completed"], out var completed)
                    ? completed
                    : DateTime.MinValue
            }).ToList();
        }

        public async Task<List<AccountDto>> GetAccountsAsync(string obpToken)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl}/obp/v5.1.0/my/accounts");
            request.Headers.Authorization = new AuthenticationHeaderValue("DirectLogin", $"token={obpToken}");

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

        public async Task<string> GetMyBanksRawAsync(string obpToken)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiUrl}/obp/v4.0.0/my/banks");
            request.Headers.Authorization = new AuthenticationHeaderValue("DirectLogin", $"token={obpToken}");

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }


        private void setTokenHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("DirectLogin", $"token={token}");
        }
    }
}
