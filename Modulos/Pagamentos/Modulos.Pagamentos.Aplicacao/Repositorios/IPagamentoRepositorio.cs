using Modulos.Pagamentos.Dominio.Entidades;

namespace Modulos.Pagamentos.Aplicacao.Repositorios;

public interface IPagamentoRepositorio
{
    Task AdicionarAsync(Pagamento pagamento);
    Task AtualizarAsync(Pagamento pagamento);
    Task<bool> ExisteAprovadoPorPedidoAsync(Guid pedidoId);
}
