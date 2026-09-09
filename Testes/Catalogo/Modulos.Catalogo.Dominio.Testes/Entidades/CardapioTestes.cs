using Bogus;
using Modulos.Catalogo.Dominio.Entidades;
using System.Globalization;

namespace Modulos.Catalogo.Dominio.Testes.Entidades
{
    public class CardapioTestes
    {
        private readonly Faker _faker;
        public CardapioTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void Criar_ComDadosValidos_DeveRetornarResultadoSucesso()
        {
            var resultadoCardapio = Cardapio.Criar(Guid.NewGuid());

            Assert.True(resultadoCardapio.Sucesso);
        }

        [Fact]
        public void Criar_ComRestauranteIdVazio_DeveRetornarResultadoFalha()
        {
            var resultadoCardapio = Cardapio.Criar(Guid.Empty);

            Assert.False(resultadoCardapio.Sucesso);
        }

        [Fact]
        public void AdicionarItem_ComDadosValidos_DeveRetornarResultadoSucesso()
        {
            var resultadoCardapio = Cardapio.Criar(Guid.NewGuid());

            var resultadoItemCardapio = resultadoCardapio.Valor.AdicionarItem(_faker.Vehicle.Model(), _faker.Lorem.Paragraphs(), _faker.PickRandom<decimal>(0, 100));

            Assert.True(resultadoItemCardapio.Sucesso);
            Assert.NotEmpty(resultadoCardapio.Valor.Itens);
        }

        [Fact]
        public void AdicionarItem_ComDadosInvalidos_DeveRetornarResultadoFalha()
        {
            var resultadoCardapio = Cardapio.Criar(Guid.NewGuid());

            var resultadoItemCardapio = resultadoCardapio.Valor.AdicionarItem(string.Empty, string.Empty, _faker.PickRandom<decimal>(-100, -1));

            Assert.False(resultadoItemCardapio.Sucesso);
            Assert.Empty(resultadoCardapio.Valor.Itens);
        }
    }
}