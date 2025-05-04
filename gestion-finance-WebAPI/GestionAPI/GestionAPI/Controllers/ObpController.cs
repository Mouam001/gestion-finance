using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GestionAPI.Controllers
{
    [ApiController]
    [Route("api/opb")]

    public class ObpController : ControllerBase
    {
        private readonly IObpService _obpService;

        public ObpController(IObpService obpService)
        {
            _obpService = obpService;
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
                return StatusCode(500, $"Erreur lors de la recuperation ds banques: {ex.Message}");
            }
        }
        
        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                var count = await _obpService.GetAccountsAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("accounts/balances/{bankId}/{accountId}")]
        public async Task<IActionResult> GetAccountDetails(string bankId, string accountId)
        {
            try
            {
                var results = await _obpService.GetAccountDetailsAsync(bankId, accountId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"Errur : {ex.Message}");
            }
        }
        
        [HttpGet("transactions/{bankId}/{accountId}")]
        public async Task<IActionResult> GetTransactions(string bankId, string accountId)
        {
            try
            {
                var results = await _obpService.GetTransactionsAsync(bankId, accountId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500,$"Erreur lors de la recuperation des transactions : {ex.Message}");
            }
        }
        
        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccount([FromQuery] string userId, [FromQuery] string bankId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(bankId))
                return BadRequest("Paramètres invalides.");

            try
            {
                var result = await _obpService.CreatAccountAsync(userId, bankId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la création du compte : {ex.Message}");
            }
        }
        
        [HttpGet("mybanks-raw")]
        public async Task<IActionResult> GetMyBanksRaw()
        {
            try
            {
                var result = await _obpService.GetMyBanksRawAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur : {ex.Message}");
            }
        }
    }
}