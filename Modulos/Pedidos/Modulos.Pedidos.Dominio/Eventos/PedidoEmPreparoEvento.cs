namespace Modulos.Pedidos.Dominio.Eventos
{
    public record PedidoEmPreparoEvento(Guid PedidoId) : IEventoDominio;
}