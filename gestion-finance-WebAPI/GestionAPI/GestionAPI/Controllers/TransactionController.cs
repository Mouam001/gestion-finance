using System.Security.Claims;
using Business.Interfaces;
using Common.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace GestionAPI.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionController(ITransactionService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> Add([FromBody] CreateTransactionRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID non trouvé dans le token");

            var userId = int.Parse(userIdClaim.Value);

            var tx = await _service.AddTransaction(userId, request);
            return Ok(tx);
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetByUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID non trouvé dans le token");

            var userId = int.Parse(userIdClaim.Value);
            var txs = await _service.GetUserTransactions(userId);
            return Ok(txs);
        }

        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> Delete(int transactionId)
        {
            var success = await _service.DeleteTransaction(transactionId);
            return success ? Ok() : NotFound();
        }
        
        [HttpPut("{transactionId}")]
        public async Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] UpdateTransactionRequest request)
        {
            try
            {
                var updatedTransaction = await _service.UpdateTransaction(transactionId, request);
                return Ok(updatedTransaction);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}