using LoginTopMed.DTOs;

namespace LoginTopMed.Services.Auth;
public interface IAuthService
{
    Task<LoginResponseDTO> AuthenticateAsync(LoginDTO loginDTO);
    //Task<LoginResponseDTO> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    //Task<bool> BlacklistTokenAsync(string jti, CancellationToken cancellationToken = default);
    //bool IsTokenBlacklisted(string jti);
    //Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
}
