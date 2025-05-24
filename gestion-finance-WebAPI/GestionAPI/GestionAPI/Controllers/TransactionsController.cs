using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;



using Common.DTO;
using Business.Implementations;

namespace GestionAPI.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("getTransactions")]
        public IActionResult ExportPdf(DateTime start, DateTime end)
        {
            var transactions = _transactionService.GetTransactions(start, end);
            var pdfBytes = PdfExporter.ExportTransactions(transactions, start, end);
            var fileName = $"transactions_{start:yyyyMMdd}_{end:yyyyMMdd}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}