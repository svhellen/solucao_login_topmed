namespace LoginTopMed.DTOs;

public class ProtectedResponseDTO
{
    public string Message { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime AccessTime { get; set; }
    public string ServerTime { get; set; } = string.Empty;
}
