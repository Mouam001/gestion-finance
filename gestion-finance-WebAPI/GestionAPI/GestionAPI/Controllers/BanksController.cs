// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Threading.Tasks;
// using Business.Implementations;

// namespace GestionAPI.Controllers
// {
//     /// <summary>
//     /// Contrôleur pour récupérer les banques depuis Open Bank Project
//     /// </summary>
//     [Route("api/banks")]
//     [ApiController]
//     public class BanksController : ControllerBase
//     {
//         private readonly ObpService _obpService;

//         public BanksController(ObpService obpService)
//         {
//             _obpService = obpService;
//         }

//         /// <summary>
//         /// 🔹 Récupère **toutes** les banques disponibles (API Open Bank Project)
//         /// </summary>
//         /// <returns>Liste de toutes les banques (y compris celles non liées à l'utilisateur)</returns>
//         [HttpGet("all")]
//         [ProducesResponseType(200)] // Code 200 si OK
//         [ProducesResponseType(500)] // Code 500 si erreur
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
//         /// 🔹 Récupère les **banques associées à l'utilisateur** (via ses comptes bancaires)
//         /// </summary>
//         /// <returns>Liste des banques où l'utilisateur possède un compte</returns>
//         [HttpGet("user")]
//         [ProducesResponseType(200)] // Code 200 si OK
//         [ProducesResponseType(500)] // Code 500 si erreur
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
        
        
        
//         [HttpPost("login")]
//         [ProducesResponseType(200)] // Code 200 si OK
//         [ProducesResponseType(400)] // Code 400 si mauvais login
//         [ProducesResponseType(500)] // Code 500 si erreur serveur
//         public async Task<IActionResult> Login([FromBody] ObpRequest obpRequest)
//         {
//             try
//             {
//                 // Authentifier l'utilisateur et récupérer le token
//                 var token = await _obpService.GetAccessTokenAsync();
                
//                 if (string.IsNullOrEmpty(token))
//                 {
//                     return BadRequest("Échec de l'authentification");
//                 }
                
//                 return Ok(new { AccessToken = token });
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Erreur lors de l'authentification : {ex.Message}");
//             }
//         }

//         [HttpPost("create")]
//         public async Task<IActionResult> CreateAccount( string userId,  string bankId,  string accountType,  double initialBalance)
//         {
//             // Vérification des paramètres
//             if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(bankId) || string.IsNullOrEmpty(accountType) || initialBalance < 0)
//             {
//                 return BadRequest("Les paramètres fournis sont invalides.");
//             }

//             try
//             {
//                 // Appel du service pour créer le compte bancaire
//                 var response = await _obpService.CreateAccountAsync(userId, bankId, accountType, initialBalance);

//                 // Retourner la réponse avec les détails du compte bancaire créé (si nécessaire)
//                 return Ok(response);
//             }
//             catch (Exception ex)
//             {
//                 // Gestion des erreurs et retour d'un message d'erreur détaillé
//                 return StatusCode(500, $"Erreur lors de la création du compte bancaire : {ex.Message}");
//             }
//         }
        
//         [HttpGet("banks")]
//         public async Task<IActionResult> GetBanks()
//         {
//             try
//             {
//                 var response = await _obpService.GetmyBanksAsync();
//                 return Ok(response);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Error: {ex.Message}");
//             }
//         }

//         // Get account details for a specific bank and account
//         [HttpGet("accounts/balances/{bankId}/{accountId}")]
//         public async Task<IActionResult> GetAccountDetails( string bankId, string accountId)
//         {
//             try
//             {
//                 var response = await _obpService.GetAccountDetailsAsync( bankId, accountId);
//                 return Ok(response);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Error: {ex.Message}");
//             }
//         }

//         [HttpGet("getTransactions")]
//         public async Task<IActionResult> GetTransactions(
//             [FromQuery] string bankId,
//             [FromQuery] string accountId,
//             [FromQuery] string viewId)
//         {
//             try
//             {
//                 var transactions = await _obpService.GetTransactionsAsync( bankId, accountId, viewId);
//                 return Ok(transactions);
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"Error retrieving transactions: {ex.Message}");
//             }
//         }
        
//         [HttpGet("getAccounts")]
//         public async Task<IActionResult> GetAccounts()
//         {
//             try
//             {
//                 var accounts = await _obpService.GetAccountsAsync();
//                 return Ok(accounts);
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


// using Business.Interfaces;
// using Common.DTO;
// using Microsoft.AspNetCore.Mvc;
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace GestionAPI.Controllers
// {
//     /// <summary>
//     /// Contrôleur pour accéder aux fonctionnalités de l’Open Bank Project (banques, comptes, transactions)
//     /// </summary>
//     [ApiController]
//     [Route("api/obp")]
//     public class ObpController : ControllerBase
//     {
//         private readonly IObpService _obpService;

//         public ObpController(IObpService obpService)
//         {
//             _obpService = obpService;
//         }

//         /// <summary>
//         /// 🔹 Liste toutes les banques disponibles via OBP
//         /// </summary>
//         [HttpGet("banks")]
//         public async Task<IActionResult> GetBanks()
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
//         /// 📘 Liste des comptes bancaires de l’utilisateur
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

//         /// <summary>
//         /// 💰 Détails d’un compte (solde, devise, etc.)
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
//         /// 📄 Transactions d’un compte bancaire spécifique
//         /// </summary>
//         [HttpGet("transactions/{bankId}/{accountId}")]
//         public async Task<IActionResult> GetTransactions(string bankId, string accountId)
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
//         /// ➕ Crée un compte bancaire pour un utilisateur
//         /// </summary>
//         [HttpPost("create-account")]
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
//         /// 🔄 Données brutes au format JSON (banques et comptes liés à l’utilisateur)
//         /// </summary>
//         [HttpGet("mybanks-raw")]
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
//     }
// }



