using Bogus;
using Modulos.Autenticacao.Aplicacao.CasosDeUso.CriarUsuario;
using Modulos.Autenticacao.Aplicacao.Repositorios;
using Modulos.Autenticacao.Aplicacao.Servicos;
using Modulos.Autenticacao.Dominio.Entidades;
using Modulos.Autenticacao.Dominio.Enums;
using Moq;
using System.Globalization;

namespace Modulos.Autenticacao.Aplicacao.Testes.CasosDeUso.CriarUsuario
{
    public class CriarUsuarioHandlerTestes
    {
        private readonly Faker _faker;
        private readonly Mock<IUsuarioRepositorio> _usuarioRepositorioMock;
        private readonly Mock<ISenhaServico> _servicoSenhaMock;

        public CriarUsuarioHandlerTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");

            _usuarioRepositorioMock = new Mock<IUsuarioRepositorio>();
            _servicoSenhaMock = new Mock<ISenhaServico>();
            _servicoSenhaMock.Setup(s => s.GerarHash(It.IsAny<string>())).Returns(_faker.Random.Hash());
        }

        [Fact]
        public async Task Handle_ComDadosValidos_DevePassarPeloAdicionarDoRepositorio()
        {
            var handler = new CriarUsuarioHandler(_usuarioRepositorioMock.Object, _servicoSenhaMock.Object);
            var command = new CriarUsuarioCommand(_faker.Person.FirstName, _faker.Person.Email, _faker.Internet.Password(), _faker.PickRandom<Papel>().ToString());

            await handler.Handle(command, CancellationToken.None);

            _usuarioRepositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ComDadosValidos_DeveHashearSenha()
        {
            var handler = new CriarUsuarioHandler(_usuarioRepositorioMock.Object, _servicoSenhaMock.Object);
            var command = new CriarUsuarioCommand(_faker.Person.FirstName, _faker.Person.Email, _faker.Internet.Password(), _faker.PickRandom<Papel>().ToString());
            Usuario usuarioCapturado = null;
            _usuarioRepositorioMock.Setup(r => r.AdicionarAsync(It.IsAny<Usuario>())).Callback<Usuario>(u => usuarioCapturado = u);

            await handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(usuarioCapturado.SenhaHash, command.Senha);
        }
    }
}
