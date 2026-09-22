namespace Modulos.Pedidos.Api.Endpoints.AdicionarItem
{
    public record AdicionarItemRequest(Guid ItemCardapioId, int Quantidade, decimal PrecoUnitario);
}