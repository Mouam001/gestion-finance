using Business.Interfaces;

namespace Business.Implementations;

using Common.DTO;

public class TransactionService : ITransactionService
{
    // En vrai, tu utiliserais probablement une base de données ici
    private readonly List<TransactionDto> _mockData = new();

    public List<TransactionDto> GetTransactions(DateTime startDate, DateTime endDate)
    {
        return _mockData
            .Where(t => t.PostedDate >= startDate && t.PostedDate <= endDate)
            .ToList();
    }

    
}
