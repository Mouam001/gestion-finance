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
        private readonly IPdfService _pdfService;

        public ObpController(IObpService obpService, IPdfService pdfService)
        {
            _obpService = obpService;
            _pdfService = pdfService;

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

        [HttpGet("pdf-releve/{bankId}/{accountId}")]
        public async Task<IActionResult> ExportObpPdf(string bankId, string accountId)
        {
            var token = Request.Headers.Authorization.ToString()?.Replace("Bearer ", "")?.Trim();
            var transactions = await _obpService.GetTransactionsAsync(token, bankId, accountId);
            if (transactions == null || transactions.Count == 0)
            {
                return NotFound(" Aucun transaction disponible");
            }
            
            if(_pdfService == null)
            {
                return StatusCode(500, "Service PDF non disponible");
            }
            
            var bytePdf = _pdfService.GenerateObpPdf(transactions, bankId);

            if (bytePdf == null)
            {
                return StatusCode(500, "erreur lors de la cratoion du Relevé bancaire");
            }
            
            return File(bytePdf, "application/pdf", $"Relevé_{bankId}_{DateTime.Now:yyyyMMdd}.pdf");
        }
        
    }
}