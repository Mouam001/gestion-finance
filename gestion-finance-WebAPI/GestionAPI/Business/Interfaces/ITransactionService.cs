using Common.DTO;

namespace Business.Interfaces;

public interface ITransactionService
{
    List<TransactionDto> GetTransactions(DateTime startDate, DateTime endDate);
}