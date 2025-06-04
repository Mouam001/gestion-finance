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
    private readonly UserDto _user;

    public TransactionsReportDocument(List<TransactionDto> transactions, DateTime startDate, DateTime endDate, UserDto user)
    {
        _transactions = transactions;
        _startDate = startDate;
        _endDate = endDate;
        _user = user;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        decimal totalIncome = _transactions
            .Where(t => t.Amount > 0)
            .Sum(t => t.Amount);

        decimal totalExpense = _transactions
            .Where(t => t.Amount < 0)
            .Sum(t => Math.Abs(t.Amount)); // Prendre la valeur absolue pour l'affichage

        decimal balance = totalIncome - totalExpense;
        decimal finalBalance = (decimal)_user.BalanceInit + balance;

        container.Page(page =>
        {
            page.Margin(20);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(x => x.FontSize(11));

            page.Header().Column(header =>
            {
                header.Item().Text("📄 Relevé des transactions").FontSize(18).Bold().AlignCenter();
                header.Item().Text($"Période : {_startDate:dd/MM/yyyy} - {_endDate:dd/MM/yyyy}").AlignCenter();
                header.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
            });

            page.Content().Column(content =>
            {
                // Informations utilisateur
                content.Item().Element(e => e.PaddingBottom(4)).Text("👤 Informations utilisateur").FontSize(14).Bold();
                content.Item().Row(row =>
                {
                    row.RelativeItem().Text($"Nom : {_user.Name} {_user.LastName}");
                    row.RelativeItem().Text($"Adresse : {_user.Address}");
                    row.RelativeItem().Text($"Solde initial : {_user.BalanceInit:N2}").FontColor(Colors.Blue.Medium);
                });

                content.Item().PaddingVertical(8).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                // Résumé
                content.Item().Element(e => e.PaddingBottom(4)).Text("💼 Résumé des transactions").FontSize(14).Bold();
                content.Item().Row(row =>
                {
                    row.RelativeItem().Text($"Recettes : {totalIncome:N2}").FontColor(Colors.Green.Darken2);
                    row.RelativeItem().Text($"Dépenses : {totalExpense:N2}").FontColor(Colors.Red.Medium);
                    row.RelativeItem().Text($"Solde final : {finalBalance:N2}").FontColor(finalBalance >= 0 ? Colors.Green.Darken3 : Colors.Red.Darken2);
                });

                content.Item().PaddingVertical(8).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                // Détails
                if (!_transactions.Any())
                {
                    content.Item().Text("Aucune transaction pour la période sélectionnée.")
                        .Italic()
                        .FontColor(Colors.Grey.Darken1);
                }
                else
                {
                    content.Item().Element(e => e.PaddingBottom(4)).Text("📋 Détail des transactions").FontSize(14).Bold();

                    content.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(80);  // Date
                            columns.RelativeColumn();    // Description
                            columns.ConstantColumn(70);  // Type
                            columns.ConstantColumn(80);  // Montant
                            columns.ConstantColumn(50);  // Devise
                            // columns.ConstantColumn(80);  // ← Pour ajouter la Catégorie
                        });

                        // En-tête
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Date").Bold();
                            header.Cell().Element(CellStyle).Text("Description").Bold();
                            header.Cell().Element(CellStyle).Text("Type").Bold();
                            header.Cell().Element(CellStyle).Text("Montant").Bold();
                            header.Cell().Element(CellStyle).Text("Devise").Bold();
                            // header.Cell().Element(CellStyle).Text("Catégorie").Bold();
                        });

                        // Données
                        foreach (var t in _transactions.OrderBy(t => t.PostedDate))
                        {
                            table.Cell().Element(CellStyle).Text(t.PostedDate.ToString());
                            table.Cell().Element(CellStyle).Text(string.IsNullOrWhiteSpace(t.Description) ? "—" : t.Description);
                            table.Cell().Element(CellStyle).Text(t.Type?.ToString() ?? "-");
                            table.Cell().Element(CellStyle).Text(t.Amount.ToString("N2"));
                            table.Cell().Element(CellStyle).Text(t.Currency?.ToString() ?? "-");
                            // table.Cell().Element(CellStyle).Text(t.Category?.ToString() ?? "-");
                        }

                        static IContainer CellStyle(IContainer container)
                        {
                            return container
                                .PaddingVertical(4)
                                .BorderBottom(1)
                                .BorderColor(Colors.Grey.Lighten2);
                        }
                    });
                }
            });

            page.Footer()
                .AlignCenter()
                .Text($"🕒 Généré le {DateTime.Now:dd/MM/yyyy à HH:mm}");
        });
    }
}
