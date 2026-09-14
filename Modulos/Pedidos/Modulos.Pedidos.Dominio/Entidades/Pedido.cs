using Compartilhado.Dominio;
using Modulos.Pedidos.Dominio.Enums;
using Modulos.Pedidos.Dominio.Eventos;

namespace Modulos.Pedidos.Dominio.Entidades
{
    public class Pedido
    {
        private Pedido(){}
        private Pedido(Guid clienteId, Guid restauranteId)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
            RestauranteId = restauranteId;
            Status = StatusPedido.Criado;
            CriadoEm = DateTime.UtcNow;
            EventosInterno.Add(new PedidoCriadoEvento(Id));
        }

        public Guid Id { get; }
        public Guid ClienteId { get; }
        public Guid RestauranteId { get; }
        public StatusPedido Status { get; private set; }
        public DateTime CriadoEm { get; }
        public IReadOnlyList<ItemPedido> Itens => ItensInterno;
        public IReadOnlyList<IEventoDominio> Eventos => EventosInterno;
        private List<ItemPedido> ItensInterno { get; } = [];
        private List<IEventoDominio> EventosInterno { get; } = [];

        public static Resultado<Pedido> Criar(Guid clienteId, Guid restauranteId)
        {
            var erros = new List<string>();
            if (clienteId == Guid.Empty)
                erros.Add("Identificador do Cliente inválido.");

            if (restauranteId == Guid.Empty)
                erros.Add("Identificador do Restaurante inválido.");

            if (erros.Count > 0)
                return Resultado<Pedido>.ComFalha(erros);

            return Resultado<Pedido>.ComSucesso(new Pedido(clienteId, restauranteId));
        }

        public Resultado<ItemPedido> AdicionarItem(Guid itemCardapioId, int quantidade, decimal precoUnitario)
        {
            if (Status != StatusPedido.Criado)
                return Resultado<ItemPedido>.ComFalha(["Status do Pedido diferente de 'Criado'."]);

            var resultadoItemPedido = ItemPedido.Criar(Id, itemCardapioId, quantidade, precoUnitario);
            if (!resultadoItemPedido.Sucesso)
                return Resultado<ItemPedido>.ComFalha(resultadoItemPedido.Erros);

            ItensInterno.Add(resultadoItemPedido.Valor);
            EventosInterno.Add(new PedidoItemAdicionadoEvento(Id));
            return Resultado<ItemPedido>.ComSucesso(resultadoItemPedido.Valor);
        }

        public Resultado<Pedido> ConfirmarPagamento()
        {
            if (Status != StatusPedido.Criado)
                return Resultado<Pedido>.ComFalha([$"Pedido em {Status} não pode ser pago."]);

            if (Itens.Count <= 0)
                return Resultado<Pedido>.ComFalha(["Pedido sem itens não pode ser pago."]);

            Status = StatusPedido.Pago;
            EventosInterno.Add(new PedidoPagoEvento(Id));

            return Resultado<Pedido>.ComSucesso(this);
        }

        public Resultado<Pedido> Aceitar()
        {
            if (Status != StatusPedido.Pago)
                return Resultado<Pedido>.ComFalha([$"Pedido em {Status} não pode ser aceito."]);

            Status = StatusPedido.Aceito;
            EventosInterno.Add(new PedidoAceitoEvento(Id));

            return Resultado<Pedido>.ComSucesso(this);
        }

        public Resultado<Pedido> IniciarPreparo()
        {
            if (Status != StatusPedido.Aceito)
                return Resultado<Pedido>.ComFalha([$"Pedido em {Status} não pode entrar em preparo."]);

            Status = StatusPedido.EmPreparo;
            EventosInterno.Add(new PedidoEmPreparoEvento(Id));

            return Resultado<Pedido>.ComSucesso(this);
        }

        public Resultado<Pedido> SairParaEntrega()
        {
            if (Status != StatusPedido.EmPreparo)
                return Resultado<Pedido>.ComFalha([$"Pedido em {Status} não pode sair para entrega."]);

            Status = StatusPedido.SaiuParaEntrega;
            EventosInterno.Add(new PedidoSaiuParaEntregaEvento(Id));

            return Resultado<Pedido>.ComSucesso(this);
        }

        public Resultado<Pedido> Entregar()
        {
            if (Status != StatusPedido.SaiuParaEntrega)
                return Resultado<Pedido>.ComFalha([$"Pedido em {Status} não pode entregar."]);

            Status = StatusPedido.Entregue;
            EventosInterno.Add(new PedidoEntregueEvento(Id));

            return Resultado<Pedido>.ComSucesso(this);
        }

        public Resultado<Pedido> Cancelar()
        {
            if (Status != StatusPedido.Criado && Status != StatusPedido.Pago)
                return Resultado<Pedido>.ComFalha([$"Pedido em {Status} não pode cancelar."]);

            Status = StatusPedido.Cancelado;
            EventosInterno.Add(new PedidoCanceladoEvento(Id));

            return Resultado<Pedido>.ComSucesso(this);
        }
    }
}