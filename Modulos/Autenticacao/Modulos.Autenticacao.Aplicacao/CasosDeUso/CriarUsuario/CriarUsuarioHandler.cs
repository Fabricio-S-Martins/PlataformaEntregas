using MediatR;
using Modulos.Autenticacao.Aplicacao.Repositorios;
using Modulos.Autenticacao.Aplicacao.Servicos;
using Modulos.Autenticacao.Dominio.Entidades;
using Modulos.Autenticacao.Dominio.Enums;

namespace Modulos.Autenticacao.Aplicacao.CasosDeUso.CriarUsuario;

public class CriarUsuarioHandler : IRequestHandler<CriarUsuarioCommand>
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly ISenhaServico _senhaServico;

    public CriarUsuarioHandler(IUsuarioRepositorio usuarioRepositorio, ISenhaServico senhaServico)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _senhaServico = senhaServico;
    }

    public async Task Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var senhaHash = _senhaServico.GerarHash(request.Senha);
        var usuario = new Usuario(request.Nome, request.Email, senhaHash, Enum.Parse<Papel>(request.Papel));

        await _usuarioRepositorio.AdicionarAsync(usuario);
    }
}