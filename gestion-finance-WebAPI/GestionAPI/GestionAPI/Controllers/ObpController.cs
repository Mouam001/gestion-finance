using Business.Interfaces;
using Common.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAPI.Controllers
{
    /// <summary>
    /// Contrôleur pour accéder aux fonctionnalités de l’Open Bank Project (banques, comptes, transactions)
    /// </summary>
    [ApiController]
    [Route("api/obp")]
    public class ObpController : ControllerBase
    {
        private readonly IObpService _obpService;

        public ObpController(IObpService obpService)
        {
            _obpService = obpService;
        }

        /// <summary>
        /// 🔹 Liste toutes les banques disponibles via OBP
        /// </summary>
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
                return StatusCode(500, $"Erreur lors de la récupération des banques : {ex.Message}");
            }
        }

        /// <summary>
        /// 📘 Liste des comptes bancaires de l’utilisateur
        /// </summary>
        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts()
        {
            try
            {
                var result = await _obpService.GetAccountsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 💰 Détails d’un compte (solde, devise, etc.)
        /// </summary>
        [HttpGet("accounts/balances/{bankId}/{accountId}")]
        public async Task<IActionResult> GetAccountDetails(string bankId, string accountId)
        {
            try
            {
                var result = await _obpService.GetAccountDetailsAsync(bankId, accountId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur : {ex.Message}");
            }
        }

        /// <summary>
        /// 📄 Transactions d’un compte bancaire spécifique
        /// </summary>
        [HttpGet("transactions/{bankId}/{accountId}")]
        public async Task<IActionResult> GetTransactions(string bankId, string accountId)
        {
            try
            {
                var result = await _obpService.GetTransactionsAsync(bankId, accountId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération des transactions : {ex.Message}");
            }
        }

        /// <summary>
        /// ➕ Crée un compte bancaire pour un utilisateur
        /// </summary>
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

        /// <summary>
        /// 🔄 Données brutes au format JSON (banques et comptes liés à l’utilisateur)
        /// </summary>
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


// using Business.Interfaces;
// using Common.DTO;
// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace GestionAPI.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class ObpController : ControllerBase
//     {
//         private readonly IObpService _obpService;

//         public ObpController(IObpService obpService)
//         {
//             _obpService = obpService;
//         }

//         /// <summary>
//         /// 🔹 Récupère toutes les banques disponibles (Open Bank Project)
//         /// </summary>
//         [HttpGet("banks")]
//         public async Task<ActionResult<List<BankDto>>> GetBanksAsync()
//         {
//             try
//             {
//                 var banks = await _obpService.GetBanksAsync();
//                 return Ok(banks);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Erreur lors de la récupération des banques : {ex.Message}");
//             }
//         }

//         /// <summary>
//         /// 🔹 Récupère les comptes bancaires de l'utilisateur
//         /// </summary>
//         [HttpGet("accounts")]
//         public async Task<ActionResult<List<AccountDto>>> GetAccounts()
//         {
//             try
//             {
//                 var accounts = await _obpService.GetAccountsAsync();
//                 return Ok(accounts);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Erreur lors de la récupération des comptes : {ex.Message}");
//             }
//         }

//         /// <summary>
//         /// 🔹 Récupère les transactions d’un compte bancaire spécifique
//         /// </summary>
//         [HttpGet("transactions/{bankId}/{accountId}")]
//         public async Task<ActionResult<List<TransactionDto>>> GetTransactions(string bankId, string accountId)
//         {
//             try
//             {
//                 var transactions = await _obpService.GetTransactionsAsync(bankId, accountId);
//                 return Ok(transactions);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Erreur lors de la récupération des transactions : {ex.Message}");
//             }
//         }

//         /// <summary>
//         /// 🔹 Récupère les banques et comptes associés à l'utilisateur au format brut JSON
//         /// </summary>
//         [HttpGet("mybanks-raw")]
//         public async Task<ActionResult<string>> GetMyBanks()
//         {
//             try
//             {
//                 var rawData = await _obpService.GetMyBanksRawAsync();
//                 return Ok(rawData);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Erreur lors de la récupération des données brutes : {ex.Message}");
//             }
//         }
//     }
// }
