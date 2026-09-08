using Bogus;
using Modulos.Autenticacao.Aplicacao.CasosDeUso.Login;
using Modulos.Autenticacao.Aplicacao.Repositorios;
using Modulos.Autenticacao.Aplicacao.Servicos;
using Modulos.Autenticacao.Dominio.Entidades;
using Modulos.Autenticacao.Dominio.Enums;
using Moq;
using System.Globalization;

namespace Modulos.Autenticacao.Aplicacao.Testes.CasosDeUso.Login
{
    public class LoginHandlerTestes
    {
        private readonly Faker _faker;
        private readonly Mock<IUsuarioRepositorio> _usuarioRepositorioMock;
        private readonly Mock<ISenhaServico> _senhaServicoMock;
        private readonly Mock<ITokenServico> _tokenServicoMock;

        public LoginHandlerTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");

            _usuarioRepositorioMock = new Mock<IUsuarioRepositorio>();
            _senhaServicoMock = new Mock<ISenhaServico>();
            _tokenServicoMock = new Mock<ITokenServico>();
        }

        [Fact]
        public async Task Handle_PassandoDadosValidos_DeveRetornarToken()
        {
            var usuario = new Usuario(_faker.Person.FirstName, _faker.Person.Email, _faker.Internet.Password(), _faker.PickRandom<Papel>());
            var senhaDigitada = _faker.Internet.Password();
            var comand = new LoginCommand(usuario.Email.Valor, senhaDigitada);
            var handler = new LoginHandler(_usuarioRepositorioMock.Object, _senhaServicoMock.Object, _tokenServicoMock.Object);

            var tokenEsperado = Guid.NewGuid().ToString();
            _usuarioRepositorioMock.Setup(r => r.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync(usuario);
            _senhaServicoMock.Setup(s => s.VerificarHash(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _tokenServicoMock.Setup(t => t.GerarToken(It.IsAny<Usuario>())).Returns(tokenEsperado);

            var token = await handler.Handle(comand, CancellationToken.None);

            Assert.Equal(tokenEsperado, token);
        }

        [Fact]
        public async Task Handle_PassandoEmailInexistente_DeveLancarExcecao()
        {
            var comand = new LoginCommand(_faker.Person.Email, _faker.Internet.Password());
            var handler = new LoginHandler(_usuarioRepositorioMock.Object, _senhaServicoMock.Object, _tokenServicoMock.Object);
            Usuario usuario = null;

            _usuarioRepositorioMock.Setup(r => r.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync(usuario);

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(comand, CancellationToken.None));
            _tokenServicoMock.Verify(t => t.GerarToken(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact]
        public async Task Handle_PassandoSenhaIncorreta_DeveLancarExcecao()
        {
            var usuario = new Usuario(_faker.Person.FirstName, _faker.Person.Email, _faker.Internet.Password(), _faker.PickRandom<Papel>());
            var senhaDigitada = _faker.Internet.Password();
            var comand = new LoginCommand(usuario.Email.Valor, senhaDigitada);
            var handler = new LoginHandler(_usuarioRepositorioMock.Object, _senhaServicoMock.Object, _tokenServicoMock.Object);

            _usuarioRepositorioMock.Setup(r => r.ObterPorEmailAsync(It.IsAny<string>())).ReturnsAsync(usuario);
            _senhaServicoMock.Setup(s => s.VerificarHash(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(comand, CancellationToken.None));
            _tokenServicoMock.Verify(t => t.GerarToken(It.IsAny<Usuario>()), Times.Never);
        }
    }
}