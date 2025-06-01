namespace Common.DTO;

/*public class TransactionDto
{
    public int Id { get; set; } 
    public int UserId { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateTime PostedDate { get; set; }
    public DateTime CompletedDate { get; set; }
}*/

public class TransactionDto
{
    public int Id { get; set; } 
    public int UserId { get; set; }
    public TransactionType? Type { get; set; }
    public TransactionCategory? Category { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public CurrencyCode? Currency { get; set; }
    public DateTime PostedDate { get; set; }
    public DateTime CompletedDate { get; set; }
}
