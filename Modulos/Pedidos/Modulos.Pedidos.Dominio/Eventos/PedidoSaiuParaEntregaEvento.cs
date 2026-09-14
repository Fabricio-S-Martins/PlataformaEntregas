namespace Modulos.Pedidos.Dominio.Eventos
{
    public record PedidoSaiuParaEntregaEvento(Guid PedidoId) : IEventoDominio;
}