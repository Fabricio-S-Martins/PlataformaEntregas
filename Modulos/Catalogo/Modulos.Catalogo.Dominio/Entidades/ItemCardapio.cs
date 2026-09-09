using Compartilhado.Dominio;

namespace Modulos.Catalogo.Dominio.Entidades
{
    public class ItemCardapio
    {
        private ItemCardapio() { }
        private ItemCardapio(Guid cardapioId, string nome, string descricao, decimal preco)
        {
            Id = Guid.NewGuid();
            CardapioId = cardapioId;
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            Disponivel = true;
        }

        public Guid Id { get; }
        public Guid CardapioId { get; }
        public string Nome { get; }
        public string Descricao { get; }
        public decimal Preco { get; }
        public bool Disponivel { get; }

        public static Resultado<ItemCardapio> Criar(Guid cardapioId, string nome, string descricao, decimal preco)
        {
            var erros = new List<string>();

            if (cardapioId == Guid.Empty)
                erros.Add("Identificador do Cardápio inválido.");

            if (string.IsNullOrWhiteSpace(nome))
                erros.Add("Nome inválido.");

            if (string.IsNullOrWhiteSpace(descricao))
                erros.Add("Descrição inválida.");

            if (preco < 0)
                erros.Add("Preço inválido.");

            if (erros.Count > 0)
                return Resultado<ItemCardapio>.ComFalha(erros);

            return Resultado<ItemCardapio>.ComSucesso(new ItemCardapio(cardapioId, nome, descricao, preco));
        }
    }
}