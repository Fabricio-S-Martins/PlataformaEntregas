using Bogus;
using Modulos.Pedidos.Dominio.Entidades;
using Modulos.Pedidos.Dominio.Enums;
using Modulos.Pedidos.Dominio.Eventos;
using System.Globalization;

namespace Modulos.Pedidos.Dominio.Testes.Entidades
{
    public class PedidoTestes
    {
        private readonly Faker _faker;
        public PedidoTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");
        }

        private Pedido CriarPedidoValido()
        {
            return Pedido.Criar(Guid.NewGuid(), Guid.NewGuid()).Valor;
        }

        private Pedido CriarPedidoComItem()
        {
            var pedido = CriarPedidoValido();
            pedido.AdicionarItem(Guid.NewGuid(), _faker.Random.Int(1, 10), _faker.PickRandom<decimal>(1, 100));
            return pedido;
        }

        private Pedido CriarPedidoPago()
        {
            var pedido = CriarPedidoComItem();
            pedido.ConfirmarPagamento();
            return pedido;
        }

        private Pedido CriarPedidoAceito()
        {
            var pedido = CriarPedidoPago();
            pedido.Aceitar();
            return pedido;
        }

        private Pedido CriarPedidoEmPreparo()
        {
            var pedido = CriarPedidoAceito();
            pedido.IniciarPreparo();
            return pedido;
        }

        private Pedido CriarPedidoSaiuParaEntrega()
        {
            var pedido = CriarPedidoEmPreparo();
            pedido.SairParaEntrega();
            return pedido;
        }

        [Fact]
        public void Criar_ComDadosValidos_DeveRetornarResultadoSucessoComStatusCriado()
        {
            var resultado = Pedido.Criar(Guid.NewGuid(), Guid.NewGuid());

            Assert.True(resultado.Sucesso);
            Assert.Equal(StatusPedido.Criado, resultado.Valor.Status);
            Assert.Empty(resultado.Valor.Itens);
            Assert.Contains(resultado.Valor.Eventos, e => e is PedidoCriadoEvento);
        }

        [Fact]
        public void Criar_ComClienteIdVazio_DeveRetornarResultadoFalha()
        {
            var resultado = Pedido.Criar(Guid.Empty, Guid.NewGuid());

            Assert.False(resultado.Sucesso);
        }

        [Fact]
        public void Criar_ComRestauranteIdVazio_DeveRetornarResultadoFalha()
        {
            var resultado = Pedido.Criar(Guid.NewGuid(), Guid.Empty);

            Assert.False(resultado.Sucesso);
        }

        [Fact]
        public void Criar_ComClienteIdERestauranteIdVazios_DeveRetornarResultadoFalhaComOsDoisErros()
        {
            var resultado = Pedido.Criar(Guid.Empty, Guid.Empty);

            Assert.False(resultado.Sucesso);
            Assert.Equal(2, resultado.Erros.Count);
        }

        [Fact]
        public void AdicionarItem_ComPedidoCriadoEDadosValidos_DeveRetornarResultadoSucessoEAdicionarNosItens()
        {
            var pedido = CriarPedidoValido();

            var resultado = pedido.AdicionarItem(Guid.NewGuid(), _faker.Random.Int(1, 10), _faker.PickRandom<decimal>(1, 100));

            Assert.True(resultado.Sucesso);
            Assert.Single(pedido.Itens);
            Assert.Contains(pedido.Eventos, e => e is PedidoItemAdicionadoEvento);
        }

        [Fact]
        public void AdicionarItem_ComDadosInvalidos_DeveRetornarResultadoFalhaSemAlterarItens()
        {
            var pedido = CriarPedidoValido();

            var resultado = pedido.AdicionarItem(Guid.Empty, 0, 0);

            Assert.False(resultado.Sucesso);
            Assert.Empty(pedido.Itens);
        }

        [Fact]
        public void AdicionarItem_ComPedidoForaDoStatusCriado_DeveRetornarResultadoFalhaSemAlterarItens()
        {
            var pedido = CriarPedidoPago();

            var resultado = pedido.AdicionarItem(Guid.NewGuid(), _faker.Random.Int(1, 10), _faker.PickRandom<decimal>(1, 100));

            Assert.False(resultado.Sucesso);
            Assert.Single(pedido.Itens);
        }

        [Fact]
        public void ConfirmarPagamento_ComPedidoCriadoComItens_DeveRetornarResultadoSucessoEMudarStatus()
        {
            var pedido = CriarPedidoComItem();

            var resultado = pedido.ConfirmarPagamento();

            Assert.True(resultado.Sucesso);
            Assert.Equal(StatusPedido.Pago, pedido.Status);
            Assert.Contains(pedido.Eventos, e => e is PedidoPagoEvento);
        }

        [Fact]
        public void ConfirmarPagamento_ComPedidoSemItens_DeveRetornarResultadoFalha()
        {
            var pedido = CriarPedidoValido();

            var resultado = pedido.ConfirmarPagamento();

            Assert.False(resultado.Sucesso);
            Assert.Equal(StatusPedido.Criado, pedido.Status);
        }

        [Fact]
        public void ConfirmarPagamento_ComPedidoForaDoStatusCriado_DeveRetornarResultadoFalha()
        {
            var pedido = CriarPedidoPago();

            var resultado = pedido.ConfirmarPagamento();

            Assert.False(resultado.Sucesso);
        }

        [Fact]
        public void Aceitar_ComPedidoPago_DeveRetornarResultadoSucessoEMudarStatus()
        {
            var pedido = CriarPedidoPago();

            var resultado = pedido.Aceitar();

            Assert.True(resultado.Sucesso);
            Assert.Equal(StatusPedido.Aceito, pedido.Status);
            Assert.Contains(pedido.Eventos, e => e is PedidoAceitoEvento);
        }

        [Fact]
        public void Aceitar_ComPedidoForaDoStatusPago_DeveRetornarResultadoFalha()
        {
            var pedido = CriarPedidoValido();

            var resultado = pedido.Aceitar();

            Assert.False(resultado.Sucesso);
        }

        [Fact]
        public void IniciarPreparo_ComPedidoAceito_DeveRetornarResultadoSucessoEMudarStatus()
        {
            var pedido = CriarPedidoAceito();

            var resultado = pedido.IniciarPreparo();

            Assert.True(resultado.Sucesso);
            Assert.Equal(StatusPedido.EmPreparo, pedido.Status);
            Assert.Contains(pedido.Eventos, e => e is PedidoEmPreparoEvento);
        }

        [Fact]
        public void IniciarPreparo_ComPedidoForaDoStatusAceito_DeveRetornarResultadoFalha()
        {
            var pedido = CriarPedidoPago();

            var resultado = pedido.IniciarPreparo();

            Assert.False(resultado.Sucesso);
        }

        [Fact]
        public void SairParaEntrega_ComPedidoEmPreparo_DeveRetornarResultadoSucessoEMudarStatus()
        {
            var pedido = CriarPedidoEmPreparo();

            var resultado = pedido.SairParaEntrega();

            Assert.True(resultado.Sucesso);
            Assert.Equal(StatusPedido.SaiuParaEntrega, pedido.Status);
            Assert.Contains(pedido.Eventos, e => e is PedidoSaiuParaEntregaEvento);
        }

        [Fact]
        public void SairParaEntrega_ComPedidoForaDoStatusEmPreparo_DeveRetornarResultadoFalha()
        {
            var pedido = CriarPedidoAceito();

            var resultado = pedido.SairParaEntrega();

            Assert.False(resultado.Sucesso);
        }

        [Fact]
        public void Entregar_ComPedidoSaiuParaEntrega_DeveRetornarResultadoSucessoEMudarStatus()
        {
            var pedido = CriarPedidoSaiuParaEntrega();

            var resultado = pedido.Entregar();

            Assert.True(resultado.Sucesso);
            Assert.Equal(StatusPedido.Entregue, pedido.Status);
            Assert.Contains(pedido.Eventos, e => e is PedidoEntregueEvento);
        }

        [Fact]
        public void Entregar_ComPedidoForaDoStatusSaiuParaEntrega_DeveRetornarResultadoFalha()
        {
            var pedido = CriarPedidoEmPreparo();

            var resultado = pedido.Entregar();

            Assert.False(resultado.Sucesso);
        }

        [Fact]
        public void Cancelar_ComPedidoCriado_DeveRetornarResultadoSucessoEMudarStatus()
        {
            var pedido = CriarPedidoValido();

            var resultado = pedido.Cancelar();

            Assert.True(resultado.Sucesso);
            Assert.Equal(StatusPedido.Cancelado, pedido.Status);
            Assert.Contains(pedido.Eventos, e => e is PedidoCanceladoEvento);
        }

        [Fact]
        public void Cancelar_ComPedidoPago_DeveRetornarResultadoSucessoEMudarStatus()
        {
            var pedido = CriarPedidoPago();

            var resultado = pedido.Cancelar();

            Assert.True(resultado.Sucesso);
            Assert.Equal(StatusPedido.Cancelado, pedido.Status);
        }

        [Fact]
        public void Cancelar_ComPedidoAceitoOuPosterior_DeveRetornarResultadoFalha()
        {
            var pedido = CriarPedidoAceito();

            var resultado = pedido.Cancelar();

            Assert.False(resultado.Sucesso);
            Assert.Equal(StatusPedido.Aceito, pedido.Status);
        }
    }
}
