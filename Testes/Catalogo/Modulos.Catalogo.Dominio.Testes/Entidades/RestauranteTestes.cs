using Bogus;
using Bogus.Extensions.Brazil;
using Modulos.Catalogo.Dominio.Entidades;
using System.Globalization;

namespace Modulos.Catalogo.Dominio.Testes.Entidades
{
    public class RestauranteTestes
    {
        private readonly Faker _faker;

        public RestauranteTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void Criar_ComDadosValidos_DeveRetornarResultadoSucesso()
        {
            var resultadoRestaurante = Restaurante.Criar(_faker.Company.CompanyName(), _faker.Company.Cnpj(false));

            Assert.True(resultadoRestaurante.Sucesso);
        }

        [Fact]
        public void Criar_ComNomeVazio_DeveRetornarResultadoFalha()
        {
            var resultadoRestaurante = Restaurante.Criar(string.Empty, _faker.Company.Cnpj(false));

            Assert.False(resultadoRestaurante.Sucesso);
        }

        [Fact]
        public void Criar_ComCnpjInvalido_DeveRetornarResultadoFalha()
        {
            var resultadoRestaurante = Restaurante.Criar(_faker.Company.CompanyName(), Guid.NewGuid().ToString());

            Assert.False(resultadoRestaurante.Sucesso);
        }

        [Fact]
        public void Criar_ComDadosInvalidos_DeveRetornarResultadoFalha()
        {
            var resultadoRestaurante = Restaurante.Criar(string.Empty, Guid.NewGuid().ToString());

            Assert.False(resultadoRestaurante.Sucesso);
            Assert.True(resultadoRestaurante.Erros.Count == 2);
        }
    }
}