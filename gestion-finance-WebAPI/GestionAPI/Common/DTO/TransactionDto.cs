namespace Common.DTO;

public class TransactionDto
{
    public int Id { get; set; }
    
    //Standard
    public int UserId { get; set; }
    public TransactionType? Type { get; set; }
    public TransactionCategory? Category { get; set; } 
    public CurrencyCode? Currency { get; set; }
    
    //OBP
    public string ObpId { get; set; }
    public string ObpType { get; set; }
    public string ObpCurrency { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
   
   
    public DateTime? PostedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
}

public class ObptransactionWrapper
{
    public string Id { get; set; }
    public ObpTransactionDetails Details { get; set; }
}

public class ObpTransactionDetails
{
    public string Type { get; set; }
    public string Description { get; set; }
    public ObpValue Value { get; set; }
    public DateTime? PostedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
}

public class ObpValue
{
    public string Currency { get; set; }
    public decimal Amount { get; set; }
   
}

