using Bogus;
using Modulos.Autenticacao.Dominio.Entidades;
using Modulos.Autenticacao.Dominio.Enums;

namespace Modulos.Autenticacao.Dominio.Testes.Entidades;

public class UsuarioTestes
{
    private readonly Faker Faker;
    public UsuarioTestes()
    {
        System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        Faker = new Faker("pt_BR");
    }

    [Fact]
    public void GerarUsuario_ComDadosValidos_DeveGerarUsuario()
    {
        var usuario = new Usuario(Faker.Person.Name, Faker.Person.Email, Faker.GetHashCode().ToString(), Faker.PickRandom<Papel>());

        Assert.NotNull(usuario);
        Assert.False(usuario.Id == Guid.Empty);
    } 

    [Fact]
    public void GerarUsuario_SomenteComEmailInvalido_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => 
            new Usuario(Faker.Person.Name, Faker.Person.Name, Faker.GetHashCode().ToString(), Faker.PickRandom<Papel>()
        ));
    }

    [Fact]
    public void GerarUsuario_ComEmailValido_DeveGerarUsuario()
    {
        var usuario = new Usuario(Faker.Person.Name, Faker.Person.Email, Faker.GetHashCode().ToString(), Faker.PickRandom<Papel>());

        Assert.NotNull(usuario);
        Assert.True(usuario.Email.Valido);
    } 
}