using Bogus;
using Modulos.Pedidos.Dominio.Entidades;
using System.Globalization;

namespace Modulos.Pedidos.Dominio.Testes.Entidades
{
    public class ItemPedidoTestes
    {
        private readonly Faker _faker;
        public ItemPedidoTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void Criar_ComDadosValidos_DeveRetornarResultadoSucesso()
        {
            var resultado = ItemPedido.Criar(Guid.NewGuid(), Guid.NewGuid(), _faker.Random.Int(1, 10), _faker.PickRandom<decimal>(1, 100));

            Assert.True(resultado.Sucesso);
        }

        [Fact]
        public void Criar_ComPedidoIdVazio_DeveRetornarResultadoFalha()
        {
            var resultado = ItemPedido.Criar(Guid.Empty, Guid.NewGuid(), _faker.Random.Int(1, 10), _faker.PickRandom<decimal>(1, 100));

            Assert.False(resultado.Sucesso);
        }

        [Fact]
        public void Criar_ComItemCardapioIdVazio_DeveRetornarResultadoFalha()
        {
            var resultado = ItemPedido.Criar(Guid.NewGuid(), Guid.Empty, _faker.Random.Int(1, 10), _faker.PickRandom<decimal>(1, 100));

            Assert.False(resultado.Sucesso);
        }

        [Fact]
        public void Criar_ComQuantidadeZeroOuNegativa_DeveRetornarResultadoFalha()
        {
            var resultado = ItemPedido.Criar(Guid.NewGuid(), Guid.NewGuid(), _faker.Random.Int(-10, 0), _faker.PickRandom<decimal>(1, 100));

            Assert.False(resultado.Sucesso);
        }

        [Fact]
        public void Criar_ComPrecoUnitarioZeroOuNegativo_DeveRetornarResultadoFalha()
        {
            var resultado = ItemPedido.Criar(Guid.NewGuid(), Guid.NewGuid(), _faker.Random.Int(1, 10), _faker.PickRandom<decimal>(-100, 0));

            Assert.False(resultado.Sucesso);
        }

        [Fact]
        public void Criar_ComTodosOsDadosInvalidos_DeveRetornarResultadoFalhaComTodosOsErros()
        {
            var resultado = ItemPedido.Criar(Guid.Empty, Guid.Empty, 0, 0);

            Assert.False(resultado.Sucesso);
            Assert.Equal(4, resultado.Erros.Count);
        }
    }
}
