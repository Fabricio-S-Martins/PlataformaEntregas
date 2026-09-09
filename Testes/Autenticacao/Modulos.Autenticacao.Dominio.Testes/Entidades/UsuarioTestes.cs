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
        public void GerarUsuario_ComDadosValidos_DeveRetornarResultadoSucesso()
        {
            var resultadoUsuario = Usuario.Criar(_faker.Person.FirstName, _faker.Person.Email, _faker.GetHashCode().ToString(), _faker.PickRandom<Papel>());

            Assert.True(resultadoUsuario.Sucesso);
        }

        [Fact]
        public void GerarUsuario_ComEmailValido_DeveGerarUsuario()
        {
            var resultadoUsuario = Usuario.Criar(_faker.Person.FirstName, _faker.Person.Email, _faker.GetHashCode().ToString(), _faker.PickRandom<Papel>());

            Assert.NotNull(resultadoUsuario.Valor);
            Assert.True(resultadoUsuario.Valor.Email.Valido);
        }

        [Fact]
        public void GerarUsuario_ComEmailInvalido_DeveRetornarResultadoFalha()
        {
            var resultadoUsuario = Usuario.Criar(_faker.Person.FirstName, _faker.Person.FirstName, _faker.GetHashCode().ToString(), _faker.PickRandom<Papel>());

            Assert.False(resultadoUsuario.Sucesso);
        }
    }
}