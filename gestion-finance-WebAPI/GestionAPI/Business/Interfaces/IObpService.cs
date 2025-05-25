using Common.DTO;

namespace Business.Interfaces
{
    public interface IObpService
    {
        Task<List<BankDto>> GetBanksAsync();
        Task<List<BankAccountDetailsDto>> GetUserBanksAsync(string obpToken);
        Task<string> CreatAccountAsync(string obpToken,string userId, string accountId);
        Task<string>GetAccountDetailsAsync(string obpToken,string bankId, string accountId);
        Task<List<TransactionDto>> GetTransactionsAsync(string obpToken,string bankId, string accountId);
        Task<List<AccountDto>> GetAccountsAsync(string obpToken);
        Task<string> GetMyBanksRawAsync(string obpToken);
       Task<string> LoginWithCredentialsAsync(string UsernameOPB, string PasswordOPB);
    }
}

