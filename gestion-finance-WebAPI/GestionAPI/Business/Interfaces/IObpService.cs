using Common.DTO;

namespace Business.Interfaces
{
    public class IObpService
    {
        Task<List<BankDto>> GetBanksAsync();
        Task<List<BankAccountDetailsDto>> GetUserBanksAsync();
        Task<string> CreatAccountAsync(string userId, string accountId);
        Task<string>GetAccountDetailsAsync(string bankId, string accountId);
        Task<List<TransactionDto>> GetTransactionsAsync(string bankId, string accountId);
        Task<List<AccountDto>> GetAccountsAsync();
        Task<string> GetMyBanksRawAsync();
    }
}

