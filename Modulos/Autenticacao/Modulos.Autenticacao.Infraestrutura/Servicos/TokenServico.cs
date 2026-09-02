using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modulos.Autenticacao.Aplicacao.Servicos;
using Modulos.Autenticacao.Dominio.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Modulos.Autenticacao.Infraestrutura.Servicos
{
    public class TokenServico : ITokenServico
    {
        private IConfiguration Configuration;

        public TokenServico(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GerarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Papel.ToString())
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Values:JWTKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt16(Configuration["Values:JWTExpires"])),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }
    }
}