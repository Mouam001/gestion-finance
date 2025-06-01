using System.Security.Claims;
using Business.Implementations;
using Business.Interfaces;
using Common.DTO;
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
        private readonly IHttpClientFactory _httpClientFactory;

        public TransactionController(ITransactionService service,IHttpClientFactory httpClientFactory)
        {
            _service = service;
            _httpClientFactory = httpClientFactory;
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
        [Authorize]
        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> Delete(int transactionId)
        {
            var success = await _service.DeleteTransaction(transactionId);
            return success ? Ok() : NotFound();
        }
        
        [Authorize]
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
        [HttpGet("getTransactions")]
        public async Task<IActionResult> ExportPdf([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
        

            var httpClient = _httpClientFactory.CreateClient("TransactionsClient");
            var requestUrl = $"/api/transactions";

            var transactions = await httpClient.GetFromJsonAsync<List<TransactionDto>>(requestUrl);

            if (transactions == null)
                return NotFound("Aucune transaction trouvée.");

            var pdfBytes = PdfExporter.ExportTransactions(transactions, start, end);
            var fileName = $"transactions_{start:yyyyMMdd}_{end:yyyyMMdd}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
    }

}