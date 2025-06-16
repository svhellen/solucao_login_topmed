namespace LoginTopMed.Services.Token;

public interface ITokenService
{
    string GenerateJwtToken(int userId, string username);
}
