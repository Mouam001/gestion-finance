using Business.Interfaces;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GestionAPI.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    public class ObpController : ControllerBase
    {
        
        private readonly IObpService _obpService;
        public ObpController(IObpService obpService)
        {
            _obpService = obpService;
        }

        [HttpGet("banks")]
        public async Task<ActionResult<List<BankDto>>> GetBanksAsync()
        {
            var banks = await _obpService.GetBanksAsync();
            return Ok(banks);
        }
        
        [HttpGet("account")]
        public async Task<ActionResult<List<AccountDto>>> GetAccounts()
        {
            var userBanks = await _obpService.GetAccountsAsync();
            return Ok(userBanks);
        }
        
        [HttpGet("transactions/{bankId}/{accountId}/{viewId}")]
        public async Task<ActionResult<List<TransactionDto>>> GetTransactions(string bankId, string accountId, string viewId)
        {
            var transactions = await _obpService.GetTransactionsAsync(bankId, accountId, viewId);
            return Ok(transactions);
        }
        
        [HttpGet("mybanks")]
        public async Task<ActionResult<List<BankAccountDetailsDto>>> GetMyBanks()
        {
            var userBanks = await _obpService.GetMyBanksRawAsync();
            return Ok(userBanks);
        }
    }
}