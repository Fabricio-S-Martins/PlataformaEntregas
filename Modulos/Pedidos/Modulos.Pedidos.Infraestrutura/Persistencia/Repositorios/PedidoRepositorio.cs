using Microsoft.EntityFrameworkCore;
using Modulos.Pedidos.Aplicacao.Repositorios;
using Modulos.Pedidos.Dominio.Entidades;

namespace Modulos.Pedidos.Infraestrutura.Persistencia.Repositorios
{
    internal class PedidoRepositorio : IPedidoRepositorio
    {
        private readonly PedidosDbContext _pedidosDbContext;

        public PedidoRepositorio(PedidosDbContext pedidosDbContext)
        {
            _pedidosDbContext = pedidosDbContext;
        }

        public async Task AdicionarAsync(Pedido pedido)
        {
            await _pedidosDbContext.AddAsync(pedido);
            await _pedidosDbContext.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Pedido pedido)
        {
            _pedidosDbContext.Update(pedido);
            await _pedidosDbContext.SaveChangesAsync();
        }

        public async Task<Pedido> ObterPorIdAsync(Guid id)
        {
            return await _pedidosDbContext.Pedidos.Include("ItensInterno")
                                                  .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}