using Compartilhado.Dominio;
using Modulos.Pagamentos.Dominio.Enums;

namespace Modulos.Pagamentos.Dominio.Entidades
{
    public class Pagamento
    {
        private Pagamento()
        { }

        private Pagamento(Guid pedidoId, decimal valor)
        {
            Id = Guid.NewGuid();
            PedidoId = pedidoId;
            Valor = valor;
            Status = StatusPagamento.Pendente;
            CriadoEm = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public Guid PedidoId { get; }
        public decimal Valor { get; }
        public StatusPagamento Status { get; private set; }
        public DateTime CriadoEm { get; }

        public static Resultado<Pagamento> Criar(Guid pedidoId, decimal valor)
        {
            var erros = new List<string>();
            if (pedidoId == Guid.Empty)
                erros.Add("Identificador do Pedido inválido.");

            if (valor <= 0)
                erros.Add("Valor inválido.");

            if (erros.Count > 0)
                return Resultado<Pagamento>.ComFalha(erros);

            return Resultado<Pagamento>.ComSucesso(new Pagamento(pedidoId, valor));
        }

        public Resultado<Pagamento> Aprovar()
        {
            if (Status != StatusPagamento.Pendente)
                return Resultado<Pagamento>.ComFalha([$"Pagamento em {Status} não pode ser aprovado."]);

            Status = StatusPagamento.Aprovado;
            return Resultado<Pagamento>.ComSucesso(this);
        }

        public Resultado<Pagamento> Recusar()
        {
            if (Status != StatusPagamento.Pendente)
                return Resultado<Pagamento>.ComFalha([$"Pagamento em {Status} não pode ser recusado."]);

            Status = StatusPagamento.Recusado;
            return Resultado<Pagamento>.ComSucesso(this);
        }

        public Resultado<Pagamento> MarcarFalha()
        {
            if (Status != StatusPagamento.Pendente)
                return Resultado<Pagamento>.ComFalha([$"Pagamento em {Status} não pode ser marcado como falho."]);

            Status = StatusPagamento.Falhou;
            return Resultado<Pagamento>.ComSucesso(this);
        }
    }
}