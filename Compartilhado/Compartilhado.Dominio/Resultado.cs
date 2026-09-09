namespace Compartilhado.Dominio
{
    public class Resultado<T>
    {
        public bool Sucesso { get; private set; }
        public T Valor { get; private set; }
        public IReadOnlyList<string> Erros { get; private set; }
        private Resultado(bool sucesso, T valor, IReadOnlyList<string> erros)
        {
            Sucesso = sucesso;
            Valor = valor;
            Erros = erros;
        }

        public static Resultado<T> ComSucesso(T valor) => new(true, valor, []);
        public static Resultado<T> ComFalha(IEnumerable<string> erros) => new (false, default, erros.ToList());
    }
}