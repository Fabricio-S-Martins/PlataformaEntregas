namespace Modulos.Catalogo.Aplicacao.CasosDeUso.ObterRestaurante
{
    public record RestauranteResponse(Guid Id, string Nome, string Cnpj, bool Ativo);
}