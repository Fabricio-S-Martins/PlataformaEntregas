using System.Text.RegularExpressions;

namespace Modulos.Autenticacao.Dominio.VOs
{
    public record Email
    {
        private static readonly Regex Regex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        public string Valor { get; }
        public bool Valido { get; }

        public Email(string valor)
        {
            Valido = !string.IsNullOrWhiteSpace(valor) && Regex.IsMatch(valor);
            Valor = valor.Trim().ToLowerInvariant();
        }
    }
}
