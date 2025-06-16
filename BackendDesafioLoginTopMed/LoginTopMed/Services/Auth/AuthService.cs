using LoginTopMed.Data;
using LoginTopMed.DTOs;
using LoginTopMed.Services.Token;
using Microsoft.EntityFrameworkCore;

namespace LoginTopMed.Services.Auth;

public class AuthService : IAuthService
{
    private readonly LoginDBContext _context;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;
    private ILogger<IAuthService> _logger;

    public AuthService(LoginDBContext context, IConfiguration configuration, ITokenService tokenService, ILogger<IAuthService> logger)
    {
        _context = context;
        _configuration = configuration;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<LoginResponseDTO> AuthenticateAsync(LoginDTO loginDTO)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(loginDTO.Username) || string.IsNullOrWhiteSpace(loginDTO.Password))
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = "Nome de usuário e senha não podem ser vazios."
                };
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDTO.Username && u.IsActive);

            if (user == null)
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = "Nome de usuário inválido."
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = "Senha inválida."
                };
            }

            var token = _tokenService.GenerateJwtToken(user.Id, user.Username);

            return new LoginResponseDTO
            {
                Success = true,
                Message = "Login bem sucedido.",
                Token = token,
                User = new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Name = user.Name,
                    Email = user.Email
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ocorreu um erro durante a autenticação: {ex.Message}");
            return new LoginResponseDTO
            {
                Success = false,
                Message = "Ocorreu um erro durante a autenticação."
            };
        }

    }   
}
