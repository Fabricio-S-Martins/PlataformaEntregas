using Modulos.Autenticacao.Dominio.Enums;
using Modulos.Autenticacao.Dominio.VOs;

namespace Modulos.Autenticacao.Dominio.Entidades;

public class Usuario
{
    private Usuario(){}
    public Usuario(string nome, string email, string senhaHash, Papel papel)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = new Email(email);
        if(!Email.Valido)
            throw new ArgumentException("E-mail inválido.", nameof(email));

        SenhaHash = senhaHash;
        Papel = papel;
    }

    public Guid Id { get; set; }
    public string Nome { get; set; }
    public Email Email { get; set; }
    public string SenhaHash { get; set; }
    public Papel Papel  { get; set; }
}