using Bogus;
using Bogus.Extensions.Brazil;
using Modulos.Catalogo.Aplicacao.CasosDeUso.CadastrarRestaurante;
using Modulos.Catalogo.Aplicacao.Repositorios;
using Modulos.Catalogo.Dominio.Entidades;
using Moq;
using System.Globalization;

namespace Modulos.Catalogo.Aplicacao.Testes.CasosDeUso.CadastrarRestaurante
{
    public class CadastrarRestauranteHandlerTestes
    {
        private readonly Faker _faker;
        private readonly Mock<IRestauranteRepositorio> _restauranteRepositorioMock;

        public CadastrarRestauranteHandlerTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");

            _restauranteRepositorioMock = new Mock<IRestauranteRepositorio>();
        }

        [Fact]
        public async Task Handle_ComDadosValidos_DevePassarPeloAdicionarDoRepositorio()
        {
            var handler = new CadastrarRestauranteHandler(_restauranteRepositorioMock.Object);
            var command = new CadastrarRestauranteCommand(_faker.Company.CompanyName(), _faker.Company.Cnpj(false));

            await handler.Handle(command, CancellationToken.None);

            _restauranteRepositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Restaurante>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ComDadosInvalidos_DeveLancarExcecaoSemChamarOAdicionarDoRepositorio()
        {
            var handler = new CadastrarRestauranteHandler(_restauranteRepositorioMock.Object);
            var command = new CadastrarRestauranteCommand(string.Empty, string.Empty);

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));

            _restauranteRepositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Restaurante>()), Times.Never);
        }
    }
}
