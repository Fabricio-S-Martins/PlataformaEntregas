using MediatR;

namespace Modulos.Autenticacao.Aplicacao.CasosDeUso.CriarUsuario;

public class CriarUsuarioCommand : IRequest
{
    public CriarUsuarioCommand(string nome, string email, string senha, string papel)
    {
        Nome = nome;
        Email = email;
        Senha = senha;
        Papel = papel;
    }

    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string Papel  { get; set; }
}