using Common.DAO;
using Common.Requests;

namespace DataAccess.Interfaces;

public interface ITransactionRepository
{
    Task<TransactionDao> AddAsync(TransactionDao transaction); 
    Task<List<TransactionDao>> GetByUserIdAsync(int userId);
    Task<TransactionDao?> GetByIdAsync(int id);
    Task<TransactionDao?> UpdateAsync(TransactionDao updated);
    Task<bool> DeleteAsync(int id);
}