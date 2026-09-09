using Compartilhado.Dominio;

namespace Modulos.Catalogo.Dominio.Entidades
{
    public class Cardapio
    {
        private Cardapio(){}
        private Cardapio(Guid restauranteId)
        {
            Id = Guid.NewGuid();
            RestauranteId = restauranteId;
        }

        public Guid Id { get; }
        public Guid RestauranteId { get; }
        public IReadOnlyList<ItemCardapio> Itens => ItensInterno;
        private List<ItemCardapio> ItensInterno { get; } = [];

        public static Resultado<Cardapio> Criar(Guid restauranteId)
        {
            var erros = new List<string>();
            if (restauranteId == Guid.Empty)
                erros.Add("Identificador do Restaurante inválido.");

            if (erros.Count > 0)
                return Resultado<Cardapio>.ComFalha(erros);

            return Resultado<Cardapio>.ComSucesso(new Cardapio(restauranteId));
        }

        public Resultado<ItemCardapio> AdicionarItem(string nome, string descricao, decimal preco)
        {
            var resultadoItemCardapio = ItemCardapio.Criar(Id, nome, descricao, preco);
            if (!resultadoItemCardapio.Sucesso)
                return Resultado<ItemCardapio>.ComFalha(resultadoItemCardapio.Erros);

            ItensInterno.Add(resultadoItemCardapio.Valor);
            return Resultado<ItemCardapio>.ComSucesso(resultadoItemCardapio.Valor);
        }
    }
}