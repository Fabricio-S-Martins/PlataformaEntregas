using Compartilhado.Dominio;

namespace Modulos.Pedidos.Dominio.Entidades
{
    public class ItemPedido
    {
        private ItemPedido() { }
        private ItemPedido(Guid pedidoId, Guid itemCardapioId, int quantidade, decimal precoUnitario)
        {
            Id = Guid.NewGuid();
            PedidoId = pedidoId;
            ItemCardapioId = itemCardapioId;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }

        public Guid Id { get; }
        public Guid PedidoId { get; }
        public Guid ItemCardapioId { get; }
        public int Quantidade { get; }
        public decimal PrecoUnitario { get; }

        public static Resultado<ItemPedido> Criar(Guid pedidoId, Guid itemCardapioId, int quantidade, decimal precoUnitario)
        {
            var erros = new List<string>();
            if (pedidoId == Guid.Empty)
                erros.Add("Identificador do pedido inválido.");

            if (itemCardapioId == Guid.Empty)
                erros.Add("Identificador do item do cardápio inválido.");

            if (quantidade <= 0)
                erros.Add("Quantidade inválida.");

            if (precoUnitario <= 0)
                erros.Add("Preço unitário inválido.");

            if (erros.Count > 0)
                return Resultado<ItemPedido>.ComFalha(erros);

            return Resultado<ItemPedido>.ComSucesso(new ItemPedido(pedidoId, itemCardapioId, quantidade, precoUnitario));
        }
    }
}