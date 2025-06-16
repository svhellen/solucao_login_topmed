using LoginTopMed.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginTopMed.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProtectedEndpointController : ControllerBase
{
    private readonly ILogger<ProtectedEndpointController> _logger;

    public ProtectedEndpointController(ILogger<ProtectedEndpointController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProtectedResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<ProtectedResponseDTO> GetProtectedData()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Desconhecido";
        var username = User.FindFirst(ClaimTypes.Name)?.Value ?? "Desconhecido";

        _logger.LogInformation("Protected endpoint accessed by user: {UserId}", userId);

        return Ok(new ProtectedResponseDTO
        {
            Message = "Você está autenticado! Este é um endpoint protegido.",
            UserId = userId,
            Username = username,
            AccessTime = DateTime.UtcNow,
            ServerTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
    }
}
