using Bogus;
using Modulos.Autenticacao.Aplicacao.CasosDeUso.CriarUsuario;
using Modulos.Autenticacao.Aplicacao.Repositorios;
using Modulos.Autenticacao.Aplicacao.Servicos;
using Modulos.Autenticacao.Dominio.Entidades;
using Modulos.Autenticacao.Dominio.Enums;
using Moq;

namespace Modulos.Autenticacao.Aplicacao.Testes.CasosDeUso.CriarUsuario;

public class CriarUsuarioHandlerTestes
{
    private readonly Faker Faker;
    private readonly Mock<IUsuarioRepositorio> UsuarioRepositorioMock;
    private readonly Mock<ISenhaServico> ServicoSenhaMock;

    public CriarUsuarioHandlerTestes()
    {
        System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        Faker = new Faker("pt_BR");
        UsuarioRepositorioMock = new Mock<IUsuarioRepositorio>();
        ServicoSenhaMock = new Mock<ISenhaServico>();
    }

    [Fact]
    public async Task Handle_PassandoDadosValidos_DevePassarPeloAdicionarDoRepositorio()
    {
        var handler = new CriarUsuarioHandler(UsuarioRepositorioMock.Object, ServicoSenhaMock.Object);
        var command = new CriarUsuarioCommand(Faker.Person.Name, Faker.Person.Email, Faker.Person.Name, Faker.PickRandom<Papel>().ToString());

        await handler.Handle(command, CancellationToken.None);

        UsuarioRepositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact]
    public async Task Handle_PassandoDadosValidos_DeveHashearSenha()
    {
        var handler = new CriarUsuarioHandler(UsuarioRepositorioMock.Object, ServicoSenhaMock.Object);
        var command = new CriarUsuarioCommand(Faker.Person.Name, Faker.Person.Email, Faker.Person.Name, Faker.PickRandom<Papel>().ToString());
        Usuario usuarioCapturado = null;
        UsuarioRepositorioMock.Setup(r => r.AdicionarAsync(It.IsAny<Usuario>()))
                              .Callback<Usuario>(u => usuarioCapturado = u);

        await handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(usuarioCapturado.SenhaHash, command.Senha);
    }
}