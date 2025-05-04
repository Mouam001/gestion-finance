namespace Common.DTO;

public class BankAccountDetailsDto
{
    public string Id { get; set; } 
    public string Label { get; set; }
    public string BankId { get; set; }
    public List<ViewDetailsDto> ViewAvailable { get; set; } 
}

public class ViewDetailsDto
{
    public string Id { get; set; }
    public string Shortname { get; set; }
    public string IsPublic { get; set; }
}