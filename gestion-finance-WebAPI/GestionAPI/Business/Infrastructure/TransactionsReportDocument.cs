namespace Business.Implementations;
using Common.DTO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;

public class TransactionsReportDocument : IDocument
{
    private readonly List<TransactionDto> _transactions;
    private readonly DateTime _startDate;
    private readonly DateTime _endDate;

    public TransactionsReportDocument(List<TransactionDto> transactions, DateTime startDate, DateTime endDate)
    {
        _transactions = transactions;
        _startDate = startDate;
        _endDate = endDate;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(20);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(12));

            page.Header()
                .Text($"Relevé des transactions : {_startDate:dd/MM/yyyy} - {_endDate:dd/MM/yyyy}")
                .FontSize(18).Bold().AlignCenter();

            page.Content().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(80);  // Date
                    columns.RelativeColumn();    // Description
                    columns.ConstantColumn(80);  // Type
                    columns.ConstantColumn(80);  // Montant
                    columns.ConstantColumn(50);  // Devise
                });

                // En-tête du tableau
                table.Header(header =>
                {
                    header.Cell().Text("Date").Bold();
                    header.Cell().Text("Description").Bold();
                    header.Cell().Text("Type").Bold();
                    header.Cell().Text("Montant").Bold();
                    header.Cell().Text("Devise").Bold();
                });

                // Contenu des lignes
                foreach (var t in _transactions)
                {
                    table.Cell().Text(t.PostedDate.ToShortDateString());
                    table.Cell().Text(t.Description);
                    table.Cell().Text(t.Type);
                    table.Cell().Text(t.Amount.ToString("N2"));
                    table.Cell().Text(t.Currency);
                }
            });

            page.Footer()
                .AlignCenter()
                .Text($"Généré le {DateTime.Now:dd/MM/yyyy à HH:mm}");
        });
    }
}
