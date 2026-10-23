namespace PracticalNCode.Domain.Entities;

public sealed class DbOperationResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ResultId { get; set; }
}

