using Business.Interfaces;
using Common.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GestionAPI.Controllers
{
    [ApiController]
    [Route("api/obp")]

    public class ObpController : ControllerBase
    {
        private readonly IObpService _obpService;

        public ObpController(IObpService obpService)
        {
            _obpService = obpService;
        }

        [HttpPost("loginOPB")]
        public async Task<IActionResult> LoginObp([FromBody] OpbLoginRequest request)
        {
            try
            {
                var token = await _obpService.LoginWithCredentialsAsync(request.UsernameOPB, request.PasswordOPB);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = " Authorisation OPB echoué", detail = ex.Message });
            }
        }
        
        [HttpGet("banks")]
        public async Task<IActionResult> GetBanks()
        {
            try
            {
                var banks = await _obpService.GetBanksAsync();
                return Ok(banks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la recuperation des banques: {ex.Message}");
            }
        }
        
        
        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts([FromHeader(Name = "Authorization")] string obpToken)
        {
            if (string.IsNullOrEmpty(obpToken))
                return BadRequest("Token OPB manquant");

            try
            {
                var cleanedToken = obpToken.Replace("Bearer ", "").Replace("DirectLogin token=", "").Trim();
                var accounts = await _obpService.GetAccountsAsync(cleanedToken);
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la recuperation des comptes: {ex.Message}");
            }
        }
       
        
        [HttpGet("accounts/balances/{bankId}/{accountId}")]
        public async Task<IActionResult> GetAccountDetails(string bankId, string accountId, [FromHeader(Name = "Authorization")] string obpToken)
        {
            if(string.IsNullOrWhiteSpace(obpToken))
                return BadRequest("Token OPB manquant");
            try
            {
                var cleanedToken = obpToken.Replace("Bearer ", "").Replace("DirectLogin token=", "").Trim();
                var results = await _obpService.GetAccountDetailsAsync(cleanedToken, bankId, accountId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"Erreur : {ex.Message}");
            }
        }
        
        [HttpGet("transactions/{bankId}/{accountId}")]
        public async Task<IActionResult> GetTransactions(string bankId, string accountId, [FromHeader(Name = "Authorization")] string obpToken)
        {
            if(string.IsNullOrWhiteSpace(obpToken))
                return BadRequest("Token OPB manquant");
            try
            {
                var cleanedToken = obpToken.Replace("Bearer ", "").Replace("DirectLogin token=", "").Trim();
                var results = await _obpService.GetTransactionsAsync(cleanedToken,bankId, accountId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"Erreur lors de la recuperation des transactions : {ex.Message}");
            }
        }
        
        
        [HttpGet("mybanks-raw")]
        public async Task<IActionResult> GetMyBanksRaw([FromHeader(Name = "Authorization")] string obpToken)
        {
            try
            {
                var cleanedToken = obpToken.Replace("Bearer ", "").Replace("DirectLogin token=", "").Trim();
                var result = await _obpService.GetMyBanksRawAsync(cleanedToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur : {ex.Message}");
            }
        }
        
        
    }
}