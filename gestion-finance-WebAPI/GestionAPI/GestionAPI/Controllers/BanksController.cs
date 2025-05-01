// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Threading.Tasks;
// using Business.Interfaces;
// using Common.DTO;

// namespace GestionAPI.Controllers
// {
//     /// <summary>
//     /// Contrôleur pour les opérations liées aux banques (Open Bank Project)
//     /// </summary>
//     [Route("api/banks")]
//     [ApiController]
//     public class BanksController : ControllerBase
//     {
//         private readonly IObpService _obpService;

//         public BanksController(IObpService obpService)
//         {
//             _obpService = obpService;
//         }

//         /// <summary>
//         /// 🔹 Récupère toutes les banques disponibles via OBP
//         /// </summary>
//         [HttpGet("all")]
//         public async Task<IActionResult> GetAllBanks()
//         {
//             try
//             {
//                 var banks = await _obpService.GetBanksAsync();
//                 return Ok(new { banks });
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, new { error = $"Erreur lors de la récupération des banques : {ex.Message}" });
//             }
//         }

//         /// <summary>
//         /// 🔹 Banques associées à l'utilisateur (où il a un compte)
//         /// </summary>
//         [HttpGet("user")]
//         public async Task<IActionResult> GetUserBanks()
//         {
//             try
//             {
//                 var userBanks = await _obpService.GetUserBanksAsync();
//                 return Ok(new { banks = userBanks });
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, new { error = $"Erreur lors de la récupération des banques utilisateur : {ex.Message}" });
//             }
//         }

//         /// <summary>
//         /// 🔐 Authentification utilisateur via OBP (retourne token)
//         /// </summary>
//         [HttpPost("login")]
//         public async Task<IActionResult> Login([FromBody] ObpRequest obpRequest)
//         {
//             try
//             {
//                 var token = await _obpService.GetMyBanksRawAsync(); // remplace GetAccessTokenAsync si tu veux juste "ping"
//                 return Ok(new { token });
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, new { error = $"Erreur d'authentification : {ex.Message}" });
//             }
//         }

//         /// <summary>
//         /// ➕ Créer un compte bancaire
//         /// </summary>
//         [HttpPost("create")]
//         public async Task<IActionResult> CreateAccount([FromQuery] string userId, [FromQuery] string bankId)
//         {
//             if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(bankId))
//                 return BadRequest("Paramètres invalides.");

//             try
//             {
//                 var result = await _obpService.CreatAccountAsync(userId, bankId);
//                 return Ok(result);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Erreur lors de la création du compte : {ex.Message}");
//             }
//         }

//         /// <summary>
//         /// 🔄 Récupère les banques de l'utilisateur au format brut JSON
//         /// </summary>
//         [HttpGet("my-banks-raw")]
//         public async Task<IActionResult> GetMyBanksRaw()
//         {
//             try
//             {
//                 var result = await _obpService.GetMyBanksRawAsync();
//                 return Ok(result);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Erreur : {ex.Message}");
//             }
//         }

//         /// <summary>
//         /// 💰 Détails d’un compte (solde, etc.)
//         /// </summary>
//         [HttpGet("accounts/balances/{bankId}/{accountId}")]
//         public async Task<IActionResult> GetAccountDetails(string bankId, string accountId)
//         {
//             try
//             {
//                 var result = await _obpService.GetAccountDetailsAsync(bankId, accountId);
//                 return Ok(result);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Erreur : {ex.Message}");
//             }
//         }

//         /// <summary>
//         /// 📄 Récupère les transactions d’un compte
//         /// </summary>
//         [HttpGet("transactions")]
//         public async Task<IActionResult> GetTransactions([FromQuery] string bankId, [FromQuery] string accountId)
//         {
//             try
//             {
//                 var result = await _obpService.GetTransactionsAsync(bankId, accountId);
//                 return Ok(result);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Erreur lors de la récupération des transactions : {ex.Message}");
//             }
//         }

//         /// <summary>
//         /// 📘 Liste des comptes utilisateur
//         /// </summary>
//         [HttpGet("accounts")]
//         public async Task<IActionResult> GetAccounts()
//         {
//             try
//             {
//                 var result = await _obpService.GetAccountsAsync();
//                 return Ok(result);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, ex.Message);
//             }
//         }
//     }

//     public class ObpRequest
//     {
//         public string Username { get; set; }
//         public string Password { get; set; }
//     }
// }
