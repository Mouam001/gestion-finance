using Business.Interfaces;
using Common.DTO;
using Common.DAO;
using System.Threading.Tasks;
using Common.Requests;
using DataAccess.Interfaces;

namespace Business.Implementations
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> RegisterUser(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetUserByEmail(request.Email);
            if (existingUser != null)
            {
                throw new Exception("Email already exists");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new UserDao()
            {
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                Password = hashedPassword,
                Address = request.Address,
                Phone = request.Phone,
                BalanceInit = 80
            };
            
            await _userRepository.AddUser(user);
            return new UserDto
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                BalanceInit = user.BalanceInit
            };
        }
    }
}