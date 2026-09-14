namespace Modulos.Pedidos.Dominio.Eventos
{
    public record PedidoAceitoEvento(Guid PedidoId) : IEventoDominio;
}
