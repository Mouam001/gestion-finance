using QuestPDF.Fluent;

namespace Business.Implementations;

using Common.DTO;

public static class PdfExporter
{
    public static byte[] ExportTransactions(List<TransactionDto> transactions, DateTime start, DateTime end)
    {
        var doc = new TransactionsReportDocument(transactions, start, end);
        return doc.GeneratePdf();
    }
}