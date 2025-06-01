using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Requests
{
    public class UpdateTransactionRequest
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public string Type { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à 0.")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; } 
        [Required]
        public string Currency { get; set; }
        public string Category { get; set; }
    }
}