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
        private IConfiguration _configuration;

        public TokenServico(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GerarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Papel.ToString())
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["APIConfiguracoes:KeyJWT"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(issuer: _configuration["APIConfiguracoes:Issuer"],
                                                   audience: _configuration["APIConfiguracoes:Audience"],
                                                   claims: claims,
                                                   expires: DateTime.UtcNow.AddMinutes(Convert.ToInt16(_configuration["APIConfiguracoes:ExpiresJWT"])),
                                                   signingCredentials: signinCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }
    }
}