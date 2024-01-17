using AspDigitalMemoSlip.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QRCoder;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenHelper
{
    private readonly string _secret;
    private readonly string _validIssuer;
    private readonly string _validAudience;
    private readonly int _tokenExpiryTimeInHour;

    public TokenHelper(IConfiguration configuration)
    {
        _secret = configuration["JWTKey:Secret"];
        _validIssuer = configuration["JWTKey:ValidIssuer"];
        _validAudience = configuration["JWTKey:ValidAudience"];
        _tokenExpiryTimeInHour = Convert.ToInt32(configuration["JWTKey:TokenExpiryTimeInHour"]);
    }

    /// <summary>
    /// Generates a JWT token for the specified claims.
    /// </summary>
    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _validIssuer,
            Audience = _validAudience,
            Expires = DateTime.UtcNow.AddHours(_tokenExpiryTimeInHour),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Generates a QR code for the specified data.
    /// </summary>
    public string GenerateQrCode(string data)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);
        return Convert.ToBase64String(qrCodeAsPngByteArr);
    }

    /// <summary>
    /// Checks if the ID of the specified consignee is about to expire.
    /// </summary>
    public bool IsIDAboutToExpire(Consignee consignee)
    {
        if (consignee.NationalRegistryExpirationDate.HasValue)
        {
            var twoMonthsFromNow = DateTime.Now.AddMonths(2);
            if (consignee.NationalRegistryExpirationDate.Value <= twoMonthsFromNow)
            {
                return true;
            }
        }
        return false;
    }


}
