using System.ComponentModel.DataAnnotations;

namespace Common.Requests
{

    public class RegisterRequest
    {
        [Required(ErrorMessage = "First name is required.")]
        public string Name { get; set; } = ""; 

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = ""; 

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required.")]
        public int Phone { get; set; } = 0;

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = "";

        [Required(ErrorMessage = "Balance is required.")]
        [Range(0.01, float.MaxValue, ErrorMessage = "Balance must be a positive number.")]
        public float BalanceInit { get; set; } = 0;
    }
}
