namespace Modulos.Pedidos.Dominio.Eventos
{
    public record PedidoPagoEvento(Guid PedidoId) : IEventoDominio;
}