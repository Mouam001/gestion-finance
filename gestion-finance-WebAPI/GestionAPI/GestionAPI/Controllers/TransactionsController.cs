using System.Net.Http;
using System.Net.Http.Json;
using Business.Implementations; // Pour .GetFromJsonAsync
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Common.DTO;

namespace GestionAPI.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TransactionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("getTransactions")]
        public async Task<IActionResult> ExportPdf([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var bankId = "123";     
            var accountId = "456";  

            var httpClient = _httpClientFactory.CreateClient("TransactionsClient");
            var requestUrl = $"/api/opb/transactions/{bankId}/{accountId}?start={start:O}&end={end:O}";

            var transactions = await httpClient.GetFromJsonAsync<List<TransactionDto>>(requestUrl);

            if (transactions == null)
                return NotFound("Aucune transaction trouvée.");

            var pdfBytes = PdfExporter.ExportTransactions(transactions, start, end);
            var fileName = $"transactions_{start:yyyyMMdd}_{end:yyyyMMdd}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}