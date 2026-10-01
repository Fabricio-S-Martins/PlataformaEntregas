using Bogus;
using Modulos.Pagamentos.Dominio.Entidades;
using Modulos.Pagamentos.Dominio.Enums;
using System.Globalization;

namespace Modulos.Pagamentos.Dominio.Testes.Entidades
{
    public class PagamentoTestes
    {
        private readonly Faker _faker;
        public PagamentoTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");
        }

        private Pagamento CriarPagamentoValido()
        {
            return Pagamento.Criar(Guid.NewGuid(), _faker.Random.Decimal(1, 1000)).Valor;
        }

        private Pagamento CriarPagamentoNoStatus(StatusPagamento status)
        {
            var pagamento = CriarPagamentoValido();
            switch (status)
            {
                case StatusPagamento.Aprovado:
                    pagamento.Aprovar();
                    break;
                case StatusPagamento.Recusado:
                    pagamento.Recusar();
                    break;
                case StatusPagamento.Falhou:
                    pagamento.MarcarFalha();
                    break;
            }
            return pagamento;
        }

        [Fact]
        public void Criar_ComDadosValidos_DeveRetornarResultadoSucessoComStatusPendente()
        {
            var resultado = Pagamento.Criar(Guid.NewGuid(), _faker.Random.Decimal(1, 1000));

            Assert.True(resultado.Sucesso);
            Assert.Equal(StatusPagamento.Pendente, resultado.Valor.Status);
            Assert.NotEqual(Guid.Empty, resultado.Valor.Id);
            Assert.NotEqual(default, resultado.Valor.CriadoEm);
        }

        [Fact]
        public void Criar_ComPedidoIdVazio_DeveRetornarResultadoFalha()
        {
            var resultado = Pagamento.Criar(Guid.Empty, _faker.Random.Decimal(1, 1000));

            Assert.False(resultado.Sucesso);
            Assert.Equal(["Identificador do Pedido inválido."], resultado.Erros);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Criar_ComValorZeroOuNegativo_DeveRetornarResultadoFalha(double valor)
        {
            var resultado = Pagamento.Criar(Guid.NewGuid(), (decimal)valor);

            Assert.False(resultado.Sucesso);
            Assert.Equal(["Valor inválido."], resultado.Erros);
        }

        [Fact]
        public void Criar_ComPedidoIdVazioEValorInvalido_DeveRetornarResultadoFalhaComOsDoisErros()
        {
            var resultado = Pagamento.Criar(Guid.Empty, 0);

            Assert.False(resultado.Sucesso);
            Assert.Equal(["Identificador do Pedido inválido.", "Valor inválido."], resultado.Erros);
        }

        [Fact]
        public void Aprovar_ComPagamentoPendente_DeveRetornarResultadoSucessoComStatusAprovado()
        {
            var pagamento = CriarPagamentoValido();

            var resultado = pagamento.Aprovar();

            Assert.True(resultado.Sucesso);
            Assert.Equal(StatusPagamento.Aprovado, pagamento.Status);
        }

        [Fact]
        public void Recusar_ComPagamentoPendente_DeveRetornarResultadoSucessoComStatusRecusado()
        {
            var pagamento = CriarPagamentoValido();

            var resultado = pagamento.Recusar();

            Assert.True(resultado.Sucesso);
            Assert.Equal(StatusPagamento.Recusado, pagamento.Status);
        }

        [Fact]
        public void MarcarFalha_ComPagamentoPendente_DeveRetornarResultadoSucessoComStatusFalhou()
        {
            var pagamento = CriarPagamentoValido();

            var resultado = pagamento.MarcarFalha();

            Assert.True(resultado.Sucesso);
            Assert.Equal(StatusPagamento.Falhou, pagamento.Status);
        }

        [Theory]
        [InlineData(StatusPagamento.Aprovado)]
        [InlineData(StatusPagamento.Recusado)]
        [InlineData(StatusPagamento.Falhou)]
        public void Aprovar_ComPagamentoFinalizado_DeveRetornarResultadoFalhaEManterStatus(StatusPagamento status)
        {
            var pagamento = CriarPagamentoNoStatus(status);

            var resultado = pagamento.Aprovar();

            Assert.False(resultado.Sucesso);
            Assert.Equal([$"Pagamento em {status} não pode ser aprovado."], resultado.Erros);
            Assert.Equal(status, pagamento.Status);
        }

        [Theory]
        [InlineData(StatusPagamento.Aprovado)]
        [InlineData(StatusPagamento.Recusado)]
        [InlineData(StatusPagamento.Falhou)]
        public void Recusar_ComPagamentoFinalizado_DeveRetornarResultadoFalhaEManterStatus(StatusPagamento status)
        {
            var pagamento = CriarPagamentoNoStatus(status);

            var resultado = pagamento.Recusar();

            Assert.False(resultado.Sucesso);
            Assert.Equal([$"Pagamento em {status} não pode ser recusado."], resultado.Erros);
            Assert.Equal(status, pagamento.Status);
        }

        [Theory]
        [InlineData(StatusPagamento.Aprovado)]
        [InlineData(StatusPagamento.Recusado)]
        [InlineData(StatusPagamento.Falhou)]
        public void MarcarFalha_ComPagamentoFinalizado_DeveRetornarResultadoFalhaEManterStatus(StatusPagamento status)
        {
            var pagamento = CriarPagamentoNoStatus(status);

            var resultado = pagamento.MarcarFalha();

            Assert.False(resultado.Sucesso);
            Assert.Equal([$"Pagamento em {status} não pode ser marcado como falho."], resultado.Erros);
            Assert.Equal(status, pagamento.Status);
        }
    }
}