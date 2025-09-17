using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmpresaX.POS.API.Services
{
    public class AuthenticationService
    {
        private readonly string _jwtKey = "ChaveSecretaEmpresaXPOSAPI2025MuitoSecreta123456";
        private readonly string _jwtIssuer = "EmpresaX.POS.API";
        private readonly string _jwtAudience = "EmpresaX.POS.Client";

        public string GenerateJwtToken(string login, string nome, string email, string empresaId, string perfil)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtKey);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("nameid", login),
                    new Claim("unique_name", login),
                    new Claim("nome", nome),
                    new Claim("email", email),
                    new Claim("empresaId", empresaId),
                    new Claim("role", perfil),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateCredentials(string login, string senha)
        {
            // Validação simples para teste
            return login == "admin" && senha == "admin123";
        }
    }
}
