namespace Modulos.Pedidos.Dominio.Eventos
{
    public record PedidoItemAdicionadoEvento(Guid PedidoId) : IEventoDominio;
}