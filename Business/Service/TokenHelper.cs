using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

public class TokenHelper
{
    public static void DecodeJwtToken(string token, string key, string issuer, string audience)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(key))
        };

        try
        {
            // Token'ı çözümleyin ve doğrulayın
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            // Token'ın içeriğini yazdırın
            Console.WriteLine("Token Claims:");
            foreach (var claim in jwtToken.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token doğrulama hatası: {ex.Message}");
        }
    }
}
