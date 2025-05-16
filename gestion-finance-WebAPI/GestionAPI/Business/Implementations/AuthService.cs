using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Interfaces;
using Common.DTO;
using Common.Requests;
using DataAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Implementations
{
    public class AuthService : IAuthservice
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthService(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public async Task<AuthenticateResponse> Login(LoginRequest request)
        {
            // Authentifier l'utilisateur
            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            Console.Write(user);

            // Si l'utilisateur est nul ou le mot de passe est incorrect
            if (user == null )
            {
                throw new Exception("Invalid email or password");
            }

            // Générer le jeton JWT
            var token = GenerateJwtToken(user);

            // Retourner la réponse avec le jeton et l'utilisateur
            return new AuthenticateResponse
            {
                Token = token,
                User = user
            };
        }

        private string GenerateJwtToken(UserDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Création des claims (informations de l'utilisateur à inclure dans le JWT)
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],  // Issuer
                _configuration["Jwt:Issuer"],  // Audience
                claims,                         // Claims
                expires: DateTime.Now.AddMinutes(30), // Durée de validité
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Générer le jeton
        }
        
        public async Task Logout()
        {
            
            await Task.CompletedTask; // Placeholder async
        }

    }
}
