namespace Modulos.Pedidos.Dominio.Eventos
{
    public record PedidoCanceladoEvento(Guid PedidoId) : IEventoDominio;
}