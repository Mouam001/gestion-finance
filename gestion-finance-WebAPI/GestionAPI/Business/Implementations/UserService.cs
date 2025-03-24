using Business.Interfaces;
using Common.DTO;
using Common.DAO;
using System.Threading.Tasks;
using Common.Requests;
using DataAccess.Implementations;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business.Implementations
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        public UserService(IUserRepository userRepository,AppDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<UserDto> RegisterUser(RegisterRequest request)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            // Hacher le mot de passe avant de le stocker (BCrypt génère automatiquement un salt)
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new UserDao()
            {
                Name = request.Name,
                Email = request.Email,
                Password = hashedPassword, // Stocker le mot de passe haché
                Address = request.Address,
                Phone = request.Phone,
                BalanceInit = request.BalanceInit
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Retourner un UserDto
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }


        
        
        
        public async Task<UserDto> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null; // Si l'utilisateur n'est pas trouvé ou le mot de passe est incorrect
            }

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        // AJout Methode supression 
        public async Task<bool> DeleteUSer(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}