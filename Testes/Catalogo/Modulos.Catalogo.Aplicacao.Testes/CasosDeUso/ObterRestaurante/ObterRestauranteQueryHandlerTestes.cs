using Moq;
using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.Extensions.Caching.Distributed;
using Modulos.Catalogo.Aplicacao.CasosDeUso.ObterRestaurante;
using Modulos.Catalogo.Aplicacao.Repositorios;
using Modulos.Catalogo.Aplicacao.Servicos;
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
        private readonly Mock<ITravaDistribuidaServico> _travaDistribuidaServicoMock;
        private readonly ObterRestauranteQueryHandler _handler;

        public ObterRestauranteQueryHandlerTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");

            _restauranteRepositorioMock = new Mock<IRestauranteRepositorio>();
            _distributedCacheMock = new Mock<IDistributedCache>();
            _travaDistribuidaServicoMock = new Mock<ITravaDistribuidaServico>();
            _handler = new ObterRestauranteQueryHandler(_distributedCacheMock.Object, _travaDistribuidaServicoMock.Object, _restauranteRepositorioMock.Object);
        }

        [Fact]
        public async Task Handle_ComIdValidoSemDadosNoCacheComTravaAdquirida_DeveRetornarRestauranteResponse()
        {
            var query = new ObterRestauranteQuery(Guid.NewGuid());
            var resultadoRestaurante = Restaurante.Criar(_faker.Person.FirstName, _faker.Company.Cnpj(false));
            ConfigurarTrava(adquirida: true);
            _distributedCacheMock.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((byte[])null);
            _restauranteRepositorioMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(resultadoRestaurante.Valor);

            var restauranteResponse = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(restauranteResponse);
            _restauranteRepositorioMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Once);
            VerificarCacheGravado(Times.Once());
            VerificarTravaLiberada(Times.Once());
        }

        [Fact]
        public async Task Handle_ComTravaAdquiridaECacheJaPopuladoNaReconsulta_DeveRetornarValorDoCache()
        {
            var query = new ObterRestauranteQuery(Guid.NewGuid());
            var restauranteEmCache = CriarRestauranteResponseEmBytes(query.Id);
            ConfigurarTrava(adquirida: true);
            _distributedCacheMock.SetupSequence(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[])null)
                .ReturnsAsync(restauranteEmCache);

            var restauranteResponse = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(restauranteResponse);
            Assert.Equal(query.Id, restauranteResponse.Id);
            _restauranteRepositorioMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Never);
            VerificarTravaLiberada(Times.Once());
        }

        [Fact]
        public async Task Handle_ComTravaNaoAdquiridaECachePopuladoNoPolling_DeveRetornarValorDoCache()
        {
            var query = new ObterRestauranteQuery(Guid.NewGuid());
            var restauranteEmCache = CriarRestauranteResponseEmBytes(query.Id);
            ConfigurarTrava(adquirida: false);
            _distributedCacheMock.SetupSequence(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[])null)
                .ReturnsAsync((byte[])null)
                .ReturnsAsync(restauranteEmCache);

            var restauranteResponse = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(restauranteResponse);
            Assert.Equal(query.Id, restauranteResponse.Id);
            _restauranteRepositorioMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Never);
            VerificarTravaLiberada(Times.Never());
        }

        [Fact]
        public async Task Handle_ComTravaNaoAdquiridaECacheNuncaPopulado_DeveConsultarBancoSemGravarCache()
        {
            var query = new ObterRestauranteQuery(Guid.NewGuid());
            var resultadoRestaurante = Restaurante.Criar(_faker.Person.FirstName, _faker.Company.Cnpj(false));
            ConfigurarTrava(adquirida: false);
            _distributedCacheMock.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((byte[])null);
            _restauranteRepositorioMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(resultadoRestaurante.Valor);

            var restauranteResponse = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(restauranteResponse);
            _restauranteRepositorioMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Once);
            VerificarCacheGravado(Times.Never());
            VerificarTravaLiberada(Times.Never());
        }

        [Fact]
        public async Task Handle_ComIdValidoComDadosNoCache_DeveRetornarRestauranteResponse()
        {
            var query = new ObterRestauranteQuery(Guid.NewGuid());
            _distributedCacheMock.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(CriarRestauranteResponseEmBytes(query.Id));

            var restauranteResponse = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(restauranteResponse);
            _restauranteRepositorioMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Never);
            _travaDistribuidaServicoMock.Verify(t => t.AdquirirAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<TimeSpan>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ComIdValidoSemDadosComTravaAdquirida_DeveRetornarNulo()
        {
            var query = new ObterRestauranteQuery(Guid.NewGuid());
            Restaurante resultadoRestaurante = null;
            ConfigurarTrava(adquirida: true);
            _distributedCacheMock.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((byte[])null);
            _restauranteRepositorioMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(resultadoRestaurante);

            var restauranteResponse = await _handler.Handle(query, CancellationToken.None);

            Assert.Null(restauranteResponse);
            _restauranteRepositorioMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>()), Times.Once);
            VerificarCacheGravado(Times.Never());
            VerificarTravaLiberada(Times.Once());
        }

        private void ConfigurarTrava(bool adquirida)
        {
            _travaDistribuidaServicoMock
                .Setup(t => t.AdquirirAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(adquirida);
        }

        private byte[] CriarRestauranteResponseEmBytes(Guid id)
        {
            var restauranteResponse = new RestauranteResponse(id, _faker.Person.FirstName, _faker.Company.Cnpj(false), true);
            return JsonSerializer.SerializeToUtf8Bytes(restauranteResponse);
        }

        private void VerificarCacheGravado(Times vezes)
        {
            _distributedCacheMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), vezes);
        }

        private void VerificarTravaLiberada(Times vezes)
        {
            _travaDistribuidaServicoMock.Verify(t => t.LiberarAsync(It.IsAny<string>(), It.IsAny<Guid>()), vezes);
        }
    }
}
