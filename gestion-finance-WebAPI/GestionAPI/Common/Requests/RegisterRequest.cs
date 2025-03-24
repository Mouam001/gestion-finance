using System.ComponentModel.DataAnnotations;

namespace Common.Requests
{

    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]  // Mot de passe doit être d'au moins 6 caractères
        public string Password { get; set; }

        [Required]
        public int Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Range(0.01, float.MaxValue)]  // balanceInit doit être un nombre positif
        public float BalanceInit { get; set; }
    }
}