namespace Modulos.Pedidos.Dominio.Eventos
{
    public record PedidoCriadoEvento(Guid PedidoId) : IEventoDominio;
}