namespace Common.DTO;

public class TransactionDto
{
    public string Id { get; set; } 
    public string Type { get; set; }
    public string Description { get; set; }
    public string Amount { get; set; }
    public string Currency { get; set; }
    public DateTime PosteDate { get; set; }
    public DateTime CompleteDate { get; set; }
}