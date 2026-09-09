using Bogus;
using Modulos.Catalogo.Dominio.Entidades;
using System.Globalization;

namespace Modulos.Catalogo.Dominio.Testes.Entidades
{
    public class ItemCardapioTestes
    {
        private readonly Faker _faker;
        public ItemCardapioTestes()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void Criar_ComDadosValidos_DeveRetornarResultadoSucesso()
        {
            var resultadoItemCardapio = ItemCardapio.Criar(Guid.NewGuid(), _faker.Vehicle.Model(), _faker.Lorem.Paragraphs(), _faker.PickRandom<decimal>(0, 100));

            Assert.True(resultadoItemCardapio.Sucesso);
            Assert.True(resultadoItemCardapio.Valor.Disponivel);
        }

        [Fact]
        public void Criar_ComCardapioIdVazio_DeveRetornarResultadoFalha()
        {
            var resultadoItemCardapio = ItemCardapio.Criar(Guid.Empty, _faker.Vehicle.Model(), _faker.Lorem.Paragraphs(), _faker.PickRandom<decimal>(0, 100));

            Assert.False(resultadoItemCardapio.Sucesso);
        }

        [Fact]
        public void Criar_ComNomeVazio_DeveRetornarResultadoFalha()
        {
            var resultadoItemCardapio = ItemCardapio.Criar(Guid.NewGuid(), string.Empty, _faker.Lorem.Paragraphs(), _faker.PickRandom<decimal>(0, 100));

            Assert.False(resultadoItemCardapio.Sucesso);
        }

        [Fact]
        public void Criar_ComDescricaoVazia_DeveRetornarResultadoFalha()
        {
            var resultadoItemCardapio = ItemCardapio.Criar(Guid.NewGuid(), _faker.Vehicle.Model(), string.Empty, _faker.PickRandom<decimal>(0, 100));

            Assert.False(resultadoItemCardapio.Sucesso);
        }

        [Fact]
        public void Criar_ComPrecoInvalido_DeveRetornarResultadoFalha()
        {
            var resultadoItemCardapio = ItemCardapio.Criar(Guid.NewGuid(), _faker.Vehicle.Model(), _faker.Lorem.Paragraphs(), _faker.PickRandom<decimal>(-100, -1));

            Assert.False(resultadoItemCardapio.Sucesso);
        }

        [Fact]
        public void Criar_ComDadosInvalido_DeveRetornarResultadoFalha()
        {
            var resultadoItemCardapio = ItemCardapio.Criar(Guid.Empty, string.Empty, string.Empty, _faker.PickRandom<decimal>(-100, -1));

            Assert.False(resultadoItemCardapio.Sucesso);
            Assert.True(resultadoItemCardapio.Erros.Count == 4);
        }
    }
}