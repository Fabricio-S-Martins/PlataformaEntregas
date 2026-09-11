using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Modulos.Catalogo.Aplicacao.CasosDeUso.ObterRestaurante;
using Modulos.Catalogo.Aplicacao.Repositorios;
using Modulos.Catalogo.Dominio.Entidades;
using System.Text.Json;

namespace Modulos.Catalogo.Aplicacao.CasosDeUso.CadastrarRestaurante
{
    public class CadastrarRestauranteHandler : IRequestHandler<CadastrarRestauranteCommand>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IRestauranteRepositorio _restauranteRepositorio;

        public CadastrarRestauranteHandler(IDistributedCache distributedCache, IRestauranteRepositorio restauranteRepositorio)
        {
            _distributedCache = distributedCache;
            _restauranteRepositorio = restauranteRepositorio;
        }

        public async Task Handle(CadastrarRestauranteCommand request, CancellationToken cancellationToken)
        {
            var resultadoRestaurante = Restaurante.Criar(request.Nome, request.Cnpj);
            if (!resultadoRestaurante.Sucesso)
                throw new ArgumentException(string.Join(Environment.NewLine, resultadoRestaurante.Erros));

            await _restauranteRepositorio.AdicionarAsync(resultadoRestaurante.Valor);

            var restauranteSerealizado = JsonSerializer.Serialize(new RestauranteResponse(resultadoRestaurante.Valor.Id, resultadoRestaurante.Valor.Nome, resultadoRestaurante.Valor.Cnpj, resultadoRestaurante.Valor.Ativo));
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            var chaveCache = $"catalogo:restaurante:{resultadoRestaurante.Valor.Id}";
            await _distributedCache.SetStringAsync(chaveCache, restauranteSerealizado, options, cancellationToken);
        }
    }
}