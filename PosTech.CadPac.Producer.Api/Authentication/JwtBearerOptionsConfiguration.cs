using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PosTech.CadPac.Producer.Api.Model;
using System.Text;

namespace PosTech.CadPac.Producer.Api.Authentication
{
    public class JwtBearerOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly AuthenticationCredentials _authCredentials;

        public JwtBearerOptionsConfiguration(IConfiguration configuration, AuthenticationCredentials authCredentials)
        {
            _authCredentials = authCredentials;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            Configure(options);
        }

        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = false,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(
                        _authCredentials.ClientSecret
                        )
                    )
            };
        }
    }
}
