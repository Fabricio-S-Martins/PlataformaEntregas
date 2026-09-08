using Bogus;
using Modulos.Autenticacao.Dominio.Entidades;
using Modulos.Autenticacao.Dominio.Enums;
using System.Globalization;

namespace Modulos.Autenticacao.Dominio.Testes.Entidades
{
    public class UsuarioTestes
    {
        private readonly Faker _faker;
        public UsuarioTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void GerarUsuario_ComDadosValidos_DeveGerarUsuario()
        {
            var usuario = new Usuario(_faker.Person.FirstName, _faker.Person.Email, _faker.GetHashCode().ToString(), _faker.PickRandom<Papel>());

            Assert.NotNull(usuario);
            Assert.False(usuario.Id == Guid.Empty);
        } 

        [Fact]
        public void GerarUsuario_SomenteComEmailInvalido_DeveLancarExcecao()
        {
            Assert.Throws<ArgumentException>(() => 
                new Usuario(_faker.Person.FirstName, _faker.Person.FirstName, _faker.GetHashCode().ToString(), _faker.PickRandom<Papel>()
            ));
        }

        [Fact]
        public void GerarUsuario_ComEmailValido_DeveGerarUsuario()
        {
            var usuario = new Usuario(_faker.Person.FirstName, _faker.Person.Email, _faker.GetHashCode().ToString(), _faker.PickRandom<Papel>());

            Assert.NotNull(usuario);
            Assert.True(usuario.Email.Valido);
        } 
    }
}
