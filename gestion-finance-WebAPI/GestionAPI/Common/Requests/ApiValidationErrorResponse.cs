namespace Common.Requests;

public class ApiValidationErrorResponse
{
    public Dictionary<string, string[]> Errors { get; set; } = new();
}