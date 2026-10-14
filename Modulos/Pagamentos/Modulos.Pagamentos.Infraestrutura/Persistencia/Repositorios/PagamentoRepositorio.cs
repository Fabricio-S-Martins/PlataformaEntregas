using Microsoft.EntityFrameworkCore;
using Modulos.Pagamentos.Aplicacao.Repositorios;
using Modulos.Pagamentos.Dominio.Entidades;
using Modulos.Pagamentos.Dominio.Enums;

namespace Modulos.Pagamentos.Infraestrutura.Persistencia.Repositorios;

internal class PagamentoRepositorio : IPagamentoRepositorio
{
    private readonly PagamentosDbContext _pagamentosDbContext;

    public PagamentoRepositorio(PagamentosDbContext pagamentosDbContext)
    {
        _pagamentosDbContext = pagamentosDbContext;
    }

    public async Task AdicionarAsync(Pagamento pagamento)
    {
        await _pagamentosDbContext.AddAsync(pagamento);
        await _pagamentosDbContext.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Pagamento pagamento)
    {
        _pagamentosDbContext.Update(pagamento);
        await _pagamentosDbContext.SaveChangesAsync();
    }

    public async Task<bool> ExisteAprovadoPorPedidoAsync(Guid pedidoId)
    {
        return await _pagamentosDbContext.Pagamentos.AnyAsync(p => p.PedidoId == pedidoId
                                                                   && p.Status == StatusPagamento.Aprovado);
    }
}
