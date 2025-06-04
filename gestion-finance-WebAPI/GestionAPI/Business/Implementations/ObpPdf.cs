using System.Globalization;
using Common.DTO;
using QuestPDF.Fluent;
using Business.Interfaces;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class ObpPdf : IPdfService
{
    public byte[] GenerateObpPdf(List<TransactionDto> transactions, string bankId)
    {
        var revenus = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
        var depenses = transactions.Where(t => t.Amount < 0).Sum(t => Math.Abs(t.Amount));
        var solde = revenus - depenses;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(50);
                page.Header().AlignCenter().Text("Relevé Bancaire").FontSize(20).Bold();
                page.Content().PaddingVertical(10).Column(col =>
                {
                    col.Spacing(5);

                    col.Item().Text($"Nom du compte : {bankId}").FontSize(10);
                    col.Item().Text(
                        $"Date génération : {DateTime.Now.ToString("dd/MM/yyyy à HH:mm", CultureInfo.InvariantCulture)}");
                    col.Item().PaddingTop(15).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2); // la Date
                            columns.RelativeColumn(2); // le TypeOBP
                            columns.RelativeColumn(4); // la Description
                            columns.RelativeColumn(2); // Le Débit
                            columns.RelativeColumn(2); // Le Crédit
                        });

                        //En tête du relevé
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Date").Bold();
                            header.Cell().Element(CellStyle).Text("Type").Bold();
                            header.Cell().Element(CellStyle).Text("Description").Bold();
                            header.Cell().Element(CellStyle).Text("Débit").Bold();
                            header.Cell().Element(CellStyle).Text("Crédit").Bold();

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2);
                            }
                        });

                        foreach (var tx in transactions)
                        {
                            var date = tx.PostedDate?.ToString("dd/MM/yyyy") ?? "NAN";
                            var debit = tx.Amount < 0 ? Math.Abs(tx.Amount).ToString("F2") : "";
                            var credit = tx.Amount >= 0 ? tx.Amount.ToString("F2") : "";
                            ;

                            table.Cell().Text(date);
                            table.Cell().Text(tx.ObpType ?? "Type inconnue");
                            table.Cell().Text(tx.Description ?? "Description vide");
                            table.Cell().Text(debit);
                            table.Cell().Text(credit);

                        }
                    });

                    col.Item().PaddingTop(20).AlignRight().Text($" Total Revenus: {revenus:F2}");
                    col.Item().AlignRight().Text($" Total Depenses: {depenses:F2}");
                    col.Item().AlignRight().Text($" Solde Actuel: {revenus:F2}").Bold();
                });

                page.Footer().AlignCenter().Text("Ton relevé, tdu mois avec amour ❤️").FontSize(10);
            });
        });
        return document.GeneratePdf();
    }
}