using MediatR;

namespace Modulos.Catalogo.Aplicacao.CasosDeUso.ObterRestaurante
{
    public record ObterRestauranteQuery(Guid Id) : IRequest<RestauranteResponse>;
}