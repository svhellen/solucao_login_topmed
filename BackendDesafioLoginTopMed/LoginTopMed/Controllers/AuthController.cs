using LoginTopMed.DTOs;
using LoginTopMed.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginTopMed.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private  readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    { 
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO loginDTO)
    {
        try
        {

            if (loginDTO == null)
            {
                _logger.LogWarning("Tentativa de login com dados nulos");
                return BadRequest(new LoginResponseDTO
                {
                    Success = false,
                    Message = "Dados de login são obrigatórios."
                });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Tentativa de login com dados inválidos para usuário: {Username}",
                    loginDTO.Username ?? "desconhecido");

                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new LoginResponseDTO
                {
                    Success = false,
                    Message = $"Dados inválidos: {string.Join(", ", errors)}"
                });
            }

            _logger.LogInformation($"Tentativa de login para usuário: {loginDTO.Username}");

            var result = await _authService.AuthenticateAsync(loginDTO);

            if (result.Success)
            {
                _logger.LogInformation($"Login bem-sucedido para usuário: {loginDTO.Username}");
                return Ok(result);
            }

            _logger.LogWarning("Falha no login para usuário: {Username}. Motivo: {Message}",
                loginDTO.Username, result.Message);

            return Unauthorized(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno durante tentativa de login para usuário: {Username}",
                loginDTO?.Username ?? "desconhecido");

            return StatusCode(StatusCodes.Status500InternalServerError, new LoginResponseDTO
            {
                Success = false,
                Message = "Erro interno do servidor. Tente novamente mais tarde."
            });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Logout()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            _logger.LogInformation("Logout realizado para usuário: {Username} (ID: {UserId})",
                username ?? "desconhecido", userId ?? "desconhecido");

            return Ok(new
            {
                success = true,
                message = "Logout realizado com sucesso.",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante logout");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                success = false,
                message = "Erro interno do servidor durante logout."
            });
        }
    }
}
