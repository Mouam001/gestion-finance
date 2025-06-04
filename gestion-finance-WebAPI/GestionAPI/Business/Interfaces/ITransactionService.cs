using Common.DTO;
using Common.Requests;

namespace Business.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto> AddTransaction(int userId, CreateTransactionRequest request);
        Task<TransactionDto> UpdateTransaction(int transactionId, UpdateTransactionRequest request);
        Task<bool> DeleteTransaction(int transactionId);
        public Task<List<TransactionDto>> GetUserTransactions(int userId);
        public Task<List<TransactionDto>> GetTransactions(DateTime startDate, DateTime endDate);
    }
}