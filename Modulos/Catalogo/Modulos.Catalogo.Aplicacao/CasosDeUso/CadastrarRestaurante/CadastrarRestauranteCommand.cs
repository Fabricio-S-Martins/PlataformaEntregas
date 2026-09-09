using MediatR;

namespace Modulos.Catalogo.Aplicacao.CasosDeUso.CadastrarRestaurante
{
    public record CadastrarRestauranteCommand(string Nome, string Cnpj) : IRequest;
}