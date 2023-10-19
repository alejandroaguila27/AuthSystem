using Microsoft.IdentityModel.Tokens;

public interface IJwtService
{
    string GenerateToken(int userId);
    TokenValidationParameters GetTokenValidationParameters();
    int ExpirationInMinutes { get; }
}
