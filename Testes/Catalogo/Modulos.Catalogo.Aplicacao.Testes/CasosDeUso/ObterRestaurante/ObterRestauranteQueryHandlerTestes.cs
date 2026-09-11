using Moq;
using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.Extensions.Caching.Distributed;
using Modulos.Catalogo.Aplicacao.CasosDeUso.ObterRestaurante;
using Modulos.Catalogo.Aplicacao.Repositorios;
using Modulos.Catalogo.Dominio.Entidades;
using System.Globalization;
using System.Text.Json;

namespace Modulos.Catalogo.Aplicacao.Testes.CasosDeUso.ObterRestaurante
{
    public class ObterRestauranteQueryHandlerTestes
    {
        private readonly Faker _faker;
        private readonly Mock<IRestauranteRepositorio> _restauranteRepositorioMock;
        private readonly Mock<IDistributedCache> _distributedCacheMock;
        public ObterRestauranteQueryHandlerTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");

            _restauranteRepositorioMock = new Mock<IRestauranteRepositorio>();
            _distributedCacheMock = new Mock<IDistributedCache>();
        }

        [Fact]
        public async Task Handle_ComIdValidoSemDadosNoCache_DeveRetornarRestauranteResponse()
        {
            var handler = new ObterRestauranteQueryHandler(_distributedCacheMock.Object, _restauranteRepositorioMock.Object);
            var id = Guid.NewGuid();
            var query = new ObterRestauranteQuery(id);
            var resultadoRestaurante = Restaurante.Criar(_faker.Person.FirstName, _faker.Company.Cnpj(false));
            _distributedCacheMock.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((byte[])null);
            _restauranteRepositorioMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(resultadoRestaurante.Valor);

            var restauranteResponse = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(restauranteResponse);
            _restauranteRepositorioMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Once);
            _distributedCacheMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ComIdValidoComDadosNoCache_DeveRetornarRestauranteResponse()
        {
            var handler = new ObterRestauranteQueryHandler(_distributedCacheMock.Object, _restauranteRepositorioMock.Object);
            var query = new ObterRestauranteQuery(Guid.NewGuid());
            var restauranteResponseEmCache = new RestauranteResponse(query.Id, _faker.Person.FirstName, _faker.Company.Cnpj(false), true);
            var restauranteResponseEmBytes = JsonSerializer.SerializeToUtf8Bytes(restauranteResponseEmCache);
            _distributedCacheMock.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(restauranteResponseEmBytes);

            var restauranteResponse = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(restauranteResponse);
            _restauranteRepositorioMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ComIdValidoSemDados_DeveRetornarNulo()
        {
            var handler = new ObterRestauranteQueryHandler(_distributedCacheMock.Object, _restauranteRepositorioMock.Object);
            var query = new ObterRestauranteQuery(Guid.NewGuid());
            Restaurante resultadoRestaurante = null;
            _distributedCacheMock.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((byte[])null);
            _restauranteRepositorioMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(resultadoRestaurante);

            var restauranteResponse = await handler.Handle(query, CancellationToken.None);

            Assert.Null(restauranteResponse);
            _distributedCacheMock.Verify(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _restauranteRepositorioMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}