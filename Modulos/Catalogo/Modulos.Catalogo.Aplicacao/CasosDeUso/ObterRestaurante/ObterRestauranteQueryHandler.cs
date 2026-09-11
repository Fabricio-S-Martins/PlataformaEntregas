using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Modulos.Catalogo.Aplicacao.Repositorios;
using System.Text.Json;

namespace Modulos.Catalogo.Aplicacao.CasosDeUso.ObterRestaurante
{
    public class ObterRestauranteQueryHandler : IRequestHandler<ObterRestauranteQuery, RestauranteResponse>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IRestauranteRepositorio _restauranteRepositorio;
        public ObterRestauranteQueryHandler(IDistributedCache distributedCache, IRestauranteRepositorio restauranteRepositorio)
        {
            _distributedCache = distributedCache;
            _restauranteRepositorio = restauranteRepositorio;
        }

        public async Task<RestauranteResponse> Handle(ObterRestauranteQuery request, CancellationToken cancellationToken)
        {
            var chaveCache = $"catalogo:restaurante:{request.Id}";
            var restauranteCache = await _distributedCache.GetAsync(chaveCache, cancellationToken);
            if (restauranteCache != null)
                return JsonSerializer.Deserialize<RestauranteResponse>(restauranteCache);

            var restaurante = await _restauranteRepositorio.ObterPorIdAsync(request.Id);
            if (restaurante == null)
                return null;

            var restauranteResponse = new RestauranteResponse(restaurante.Id, restaurante.Nome, restaurante.Cnpj, restaurante.Ativo);
            var restauranteSerealizado = JsonSerializer.Serialize(restauranteResponse);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            await _distributedCache.SetStringAsync(chaveCache, restauranteSerealizado, options, cancellationToken);
            return restauranteResponse;
        }
    }
}