using Compartilhado.Dominio;
using Modulos.Autenticacao.Dominio.Enums;
using Modulos.Autenticacao.Dominio.VOs;

namespace Modulos.Autenticacao.Dominio.Entidades
{
    public class Usuario
    {
        private Usuario(){}

        private Usuario(string nome, Email email, string senhaHash, Papel papel)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Email = email;
            SenhaHash = senhaHash;
            Papel = papel;
        }

        public Guid Id { get; }
        public string Nome { get; }
        public Email Email { get; }
        public string SenhaHash { get; }
        public Papel Papel { get; }

        public static Resultado<Usuario> Criar(string nome, string email, string senhaHash, Papel papel)
        {
            var erros = new List<string>();
            if (string.IsNullOrWhiteSpace(nome))
                erros.Add("Nome inválido.");

            var emailVO = new Email(email);
            if (!emailVO.Valido)
                erros.Add("E-mail inválido.");

            if (string.IsNullOrWhiteSpace(senhaHash))
                erros.Add("Senha inválida.");

            if(erros.Count > 0)
                return Resultado<Usuario>.ComFalha(erros);

            return Resultado<Usuario>.ComSucesso(new Usuario(nome, emailVO, senhaHash, papel));
        }
    }
}