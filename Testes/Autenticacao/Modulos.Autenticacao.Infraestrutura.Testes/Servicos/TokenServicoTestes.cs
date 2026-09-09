using Bogus;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modulos.Autenticacao.Dominio.Entidades;
using Modulos.Autenticacao.Dominio.Enums;
using Modulos.Autenticacao.Infraestrutura.Servicos;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Modulos.Autenticacao.Infraestrutura.Testes.Servicos
{
    public class TokenServicoTestes
    {
        private readonly Faker _faker;
        private readonly TokenServico _tokenServico;
        private const string Issuer = "https://localhost:5001";
        private const string Audience = "https://localhost:5001";
        private const string KeyJWT = "umaChaveSuperGrandeParaTeste123456";
        private const string ExpiresJWT = "5";

        public TokenServicoTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");

            var configuracaoEmMemoria = new Dictionary<string, string> {
                {"APIConfiguracoes:Issuer", Issuer},
                {"APIConfiguracoes:Audience", Audience},
                {"APIConfiguracoes:KeyJWT", KeyJWT},
                {"APIConfiguracoes:ExpiresJWT", ExpiresJWT}
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(configuracaoEmMemoria).Build();
            _tokenServico = new TokenServico(configuration);
        }

        [Fact]
        public void GerarToken_ComUsuarioValido_DeveRetornarTokenComTresSegmentos()
        {
            var resultadoUsuario = Usuario.Criar(_faker.Person.FirstName, _faker.Person.Email, _faker.Internet.Password(), _faker.PickRandom<Papel>());

            var token = _tokenServico.GerarToken(resultadoUsuario.Valor);

            Assert.Equal(2, token.Count(t => t == '.'));
        }

        [Fact]
        public void GerarToken_ComUsuarioValido_DeveIncluirIdDoUsuarioNoToken()
        {
            var resultadoUsuario = Usuario.Criar(_faker.Person.FirstName, _faker.Person.Email, _faker.Internet.Password(), _faker.PickRandom<Papel>());

            var token = _tokenServico.GerarToken(resultadoUsuario.Valor);
            var tokenDeSeguranca = new JwtSecurityTokenHandler().ReadJwtToken(token);

            Assert.Equal(tokenDeSeguranca.Subject, resultadoUsuario.Valor.Id.ToString());
        }

        [Fact]
        public void GerarToken_ComUsuarioValido_DeveIncluirPapelDoUsuarioNoToken()
        {
            var resultadoUsuario = Usuario.Criar(_faker.Person.FirstName, _faker.Person.Email, _faker.Internet.Password(), _faker.PickRandom<Papel>());

            var token = _tokenServico.GerarToken(resultadoUsuario.Valor);
            var tokenDeSeguranca = new JwtSecurityTokenHandler().ReadJwtToken(token);

            Assert.Contains(tokenDeSeguranca.Claims, c => c.Value == resultadoUsuario.Valor.Papel.ToString());
        }

        [Fact]
        public void GerarToken_ComUsuarioValido_DeveSerValidadoComSucesso()
        {
            var resultadoUsuario = Usuario.Criar(_faker.Person.FirstName, _faker.Person.Email, _faker.Internet.Password(), _faker.PickRandom<Papel>());
            var token = _tokenServico.GerarToken(resultadoUsuario.Valor);
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KeyJWT))
            };

            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            Assert.True(principal.Identity.IsAuthenticated);
        }

        [Fact]
        public void GerarToken_ComUsuarioValido_DeveExpirarConformeConfiguracao()
        {
            var dataCriacao = DateTime.UtcNow;
            var resultadoUsuario = Usuario.Criar(_faker.Person.FirstName, _faker.Person.Email, _faker.Internet.Password(), _faker.PickRandom<Papel>());

            var token = _tokenServico.GerarToken(resultadoUsuario.Valor);
            var tokenDeSeguranca = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var expiracaoEsperada = dataCriacao.AddMinutes(5);
            Assert.True(Math.Abs((tokenDeSeguranca.ValidTo - expiracaoEsperada).TotalSeconds) < 5);
        }
    }
}