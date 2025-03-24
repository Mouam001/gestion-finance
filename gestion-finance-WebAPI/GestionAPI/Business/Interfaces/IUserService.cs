using Common.DTO;
using Common.DAO;
using Common.Requests;

namespace Business.Interfaces
{

    public interface IUserService
    {
       Task<UserDto> RegisterUser (RegisterRequest request);
       Task<UserDto> AuthenticateAsync(string email, string password); // Authentification
    }
}