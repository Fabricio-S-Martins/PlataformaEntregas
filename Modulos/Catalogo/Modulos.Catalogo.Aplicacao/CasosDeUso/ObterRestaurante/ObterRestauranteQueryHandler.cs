using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Modulos.Catalogo.Aplicacao.Repositorios;
using Modulos.Catalogo.Aplicacao.Servicos;
using System.Text.Json;

namespace Modulos.Catalogo.Aplicacao.CasosDeUso.ObterRestaurante
{
    public class ObterRestauranteQueryHandler : IRequestHandler<ObterRestauranteQuery, RestauranteResponse>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ITravaDistribuidaServico _travaDistribuidaServico;
        private readonly IRestauranteRepositorio _restauranteRepositorio;

        public ObterRestauranteQueryHandler(IDistributedCache distributedCache, ITravaDistribuidaServico travaDistribuidaServico, IRestauranteRepositorio restauranteRepositorio)
        {
            _distributedCache = distributedCache;
            _travaDistribuidaServico = travaDistribuidaServico;
            _restauranteRepositorio = restauranteRepositorio;
        }

        public async Task<RestauranteResponse> Handle(ObterRestauranteQuery request, CancellationToken cancellationToken)
        {
            var chaveCache = $"catalogo:restaurante:{request.Id}";
            var restauranteCache = await ObterRestauranteCache(chaveCache, cancellationToken);
            if (restauranteCache != null)
                return restauranteCache;

            var chaveTrava = $"catalogo:restaurante:lock:{request.Id}";
            var tokenTrava = Guid.NewGuid();

            var travaAdquirida = await _travaDistribuidaServico.AdquirirAsync(chaveTrava, tokenTrava, TimeSpan.FromSeconds(5));
            if (travaAdquirida)
                return await ObterComTravaAsync(request.Id, chaveCache, chaveTrava, tokenTrava, cancellationToken);

            return await ObterComPollingAsync(request.Id, chaveCache, cancellationToken);
        }

        private async Task AdicionarRestauranteCache(string chaveCache, RestauranteResponse restauranteResponse, CancellationToken cancellationToken)
        {
            var restauranteSerializado = JsonSerializer.Serialize(restauranteResponse);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            await _distributedCache.SetStringAsync(chaveCache, restauranteSerializado, options, cancellationToken);
        }
        private async Task<RestauranteResponse> ObterComTravaAsync(Guid id, string chaveCache, string chaveTrava, Guid tokenTrava, CancellationToken cancellationToken)
        {
            try
            {
                var restauranteCache = await ObterRestauranteCache(chaveCache, cancellationToken);
                if (restauranteCache != null)
                    return restauranteCache;

                var restauranteResponse = await ObterRestaurante(id);
                if (restauranteResponse == null)
                    return null;

                await AdicionarRestauranteCache(chaveCache, restauranteResponse, cancellationToken);
                return restauranteResponse;
            }
            finally
            {
                await _travaDistribuidaServico.LiberarAsync(chaveTrava, tokenTrava);
            }
        }

        private async Task<RestauranteResponse> ObterComPollingAsync(Guid id, string chaveCache, CancellationToken cancellationToken)
        {
            for (int i = 1; i <= 5; i++)
            {
                var restauranteCache = await ObterRestauranteCache(chaveCache, cancellationToken);
                if (restauranteCache != null)
                    return restauranteCache;

                await Task.Delay(200, cancellationToken);
            }

            return await ObterRestaurante(id);
        }

        private async Task<RestauranteResponse> ObterRestauranteCache(string chaveCache, CancellationToken cancellationToken)
        {
            var retornoCache = await _distributedCache.GetStringAsync(chaveCache, cancellationToken);

            if (string.IsNullOrEmpty(retornoCache))
                return null;

            return JsonSerializer.Deserialize<RestauranteResponse>(retornoCache);
        }

        private async Task<RestauranteResponse> ObterRestaurante(Guid id)
        {
            var restaurante = await _restauranteRepositorio.ObterPorIdAsync(id);
            if (restaurante == null)
                return null;

            return new RestauranteResponse(restaurante.Id, restaurante.Nome, restaurante.Cnpj, restaurante.Ativo);
        }
    }
}