using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class JwtService : IJwtService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    public int ExpirationInMinutes { get; }

    public JwtService(IConfiguration configuration)
    {
        _secretKey = configuration["JwtSettings:SecretKey"];
        _issuer = configuration["JwtSettings:Issuer"];
        ExpirationInMinutes = int.Parse(configuration["JwtSettings:ExpirationInMinutes"]);
    }

    public string GenerateToken(int userId)
    {
        var securityKey = new SymmetricSecurityKey(GenerateKeyBytes());
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            _issuer,
            _issuer,
            claims,
            expires: DateTime.UtcNow.AddMinutes(ExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private byte[] GenerateKeyBytes()
    {
        var keyBytes = new byte[_secretKey.Length * sizeof(char)];
        Buffer.BlockCopy(_secretKey.ToCharArray(), 0, keyBytes, 0, keyBytes.Length);
        return keyBytes;
    }

    public TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(GenerateKeyBytes())
        };
    }
}
