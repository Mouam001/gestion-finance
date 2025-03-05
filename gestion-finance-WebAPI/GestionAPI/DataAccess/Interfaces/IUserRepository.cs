
using Common.DAO;

namespace DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDao> GetUserByEmail(string email);
        Task AddUser(UserDao user);
    }
}