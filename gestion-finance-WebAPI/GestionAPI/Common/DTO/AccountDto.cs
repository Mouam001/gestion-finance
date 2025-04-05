namespace Common.DTO;

public class AccountDto
{
    public string Id { get; set; } 
    public string Label { get; set; }
    public string BankId { get; set; }
    public List<string> Views { get; set; }
}