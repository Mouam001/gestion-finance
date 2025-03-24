namespace Common.Requests;

public class UpdateUserRequest
{
    
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; } = null!;
    public int? Phone { get; set; }
    public string? Address { get; set; }
    
}