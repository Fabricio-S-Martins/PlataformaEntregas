using Modulos.Pedidos.Dominio.Entidades;

namespace Modulos.Pedidos.Aplicacao.Repositorios
{
    public interface IPedidoRepositorio
    {
        Task AdicionarAsync(Pedido pedido);
        Task AtualizarAsync(Pedido pedido);
        Task<Pedido> ObterPorIdAsync(Guid id);
    }
}