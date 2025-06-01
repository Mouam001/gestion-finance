public class CreateTransactionRequest
{
    
    public string Description { get; set; }
    public string Type { get; set; }
    public string Category { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    
    public DateTime PostedDate { get; set; }
    public DateTime CompletedDate { get; set; }
    
   
}