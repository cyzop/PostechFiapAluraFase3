using Microsoft.IdentityModel.Tokens;
using PosTech.CadPac.Domain.Services;
using PosTech.CadPac.Producer.Api.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PosTech.CadPac.Producer.Api.Service
{
    public class TokenService : ITokenService
    {
        private readonly AuthenticationCredentials _authCredential;
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration, AuthenticationCredentials authCredential)
        {
            _configuration = configuration;
            _authCredential = authCredential;
        }

        public string GenerateDynamicSecret()
        {
            string dynamicSecret = string.Empty;
            var date = DateTime.Now;
            //ANO + MES | valor base | ANO - DIA
            var initialPart = date.Year + date.Month;
            var finalPart = date.Year - date.Day;

            var baseValue = string.Concat(initialPart, _authCredential.BaseHash, finalPart);

            using (var sha256 = new System.Security.Cryptography.SHA256Managed())
            {
                var keyValue = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(baseValue))).Replace("-", "");
                return keyValue;
            }
        }

        public object CreateAuthenticationToken(string clientId)
        {
            var key = Encoding.ASCII.GetBytes(_authCredential.ClientSecret);
            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                          new Claim("clientId", clientId),
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            var tokenString = tokenHandler.WriteToken(token);

            return new
            {
                access_token = tokenString,
                expires_in = ((DateTime)tokenConfig.Expires).Ticks,
            };
        }
    }
}
