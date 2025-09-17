using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmpresaX.POS.API.Services
{
    public interface IJwtService
    {
        string GerarToken(string usuario, string nome, string email, string empresaId, string perfil);
        bool ValidarToken(string token);
        ClaimsPrincipal? ObterClaimsDeToken(string token);
        DateTime ObterDataExpiracao();
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtService> _logger;

        public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GerarToken(string usuario, string nome, string email, string empresaId, string perfil)
        {
            var secretKey = "MinhaChaveSecretaSuperSeguraComMaisDe32Caracteres123456789";
            var key = Encoding.UTF8.GetBytes(secretKey);
            
            var dataExpiracao = ObterDataExpiracao();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario),
                new Claim(ClaimTypes.Name, usuario),
                new Claim("nome", nome),
                new Claim("email", email),
                new Claim("empresaId", empresaId),
                new Claim(ClaimTypes.Role, perfil),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, 
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), 
                    ClaimValueTypes.Integer64)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = dataExpiracao,
                Issuer = "EmpresaX.POS.API",
                Audience = "EmpresaX.POS.Client",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            _logger.LogInformation($"Token JWT gerado para usuário: {usuario}");
            
            return tokenHandler.WriteToken(token);
        }

        public bool ValidarToken(string token)
        {
            try
            {
                var secretKey = "MinhaChaveSecretaSuperSeguraComMaisDe32Caracteres123456789";
                var key = Encoding.UTF8.GetBytes(secretKey);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = "EmpresaX.POS.API",
                    ValidateAudience = true,
                    ValidAudience = "EmpresaX.POS.Client",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Token inválido: {ex.Message}");
                return false;
            }
        }

        public ClaimsPrincipal? ObterClaimsDeToken(string token)
        {
            try
            {
                var secretKey = "MinhaChaveSecretaSuperSeguraComMaisDe32Caracteres123456789";
                var key = Encoding.UTF8.GetBytes(secretKey);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public DateTime ObterDataExpiracao()
        {
            return DateTime.UtcNow.AddHours(8); // Token expira em 8 horas
        }
    }
}
