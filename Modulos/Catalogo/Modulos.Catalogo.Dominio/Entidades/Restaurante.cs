using Compartilhado.Dominio;

namespace Modulos.Catalogo.Dominio.Entidades
{
    public class Restaurante
    {
        private Restaurante(){}
        private Restaurante(string nome, string cnpj)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Cnpj = cnpj;
            Ativo = true;
        }

        public Guid Id { get; }
        public string Nome { get; }
        public string Cnpj { get; }
        public bool Ativo { get; }

        public static Resultado<Restaurante> Criar(string nome, string cnpj)
        {
            var erros = new List<string>();
            if (string.IsNullOrWhiteSpace(nome))
                erros.Add("Nome inválido.");

            if (cnpj?.Length != 14 || !cnpj.All(char.IsDigit))
                erros.Add("Cnpj inválido.");

            if (erros.Count > 0)
                return Resultado<Restaurante>.ComFalha(erros);

            return Resultado<Restaurante>.ComSucesso(new Restaurante(nome, cnpj));
        }
    }
}