﻿namespace LoginTopMed.DTOs;

public class LoginResponseDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public UserDTO? User { get; set; }
}
