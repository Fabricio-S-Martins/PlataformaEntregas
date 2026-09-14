namespace Modulos.Pedidos.Dominio.Eventos
{
    public record PedidoEntregueEvento(Guid PedidoId) : IEventoDominio;
}