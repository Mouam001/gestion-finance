using Common.DTO;
using Common.DAO;
using Common.Requests;

namespace Business.Interfaces
{

    public interface IUserService
    {
       Task<UserDto> RegisterUser (RegisterRequest request);
       Task<UserDto> AuthenticateAsync(string email, string password); // Authentification
       Task<bool> DeleteUSer (int userId); // Supression 
       Task<UserDto> UpdateUser (int userId, UpdateUserRequest request); // Mise à jour
       Task<List<UserDto>> GetAllUsers (); // Récupérer tous les utilisateurs
       Task<UserDto?> GetCurrentUserAsync(string jwtToken);
    }
}