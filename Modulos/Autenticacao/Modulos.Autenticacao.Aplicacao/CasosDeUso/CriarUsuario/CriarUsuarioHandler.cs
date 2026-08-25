using System.Security.Cryptography;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Modulos.Autenticacao.Aplicacao.Repositorios;
using Modulos.Autenticacao.Dominio.Entidades;
using Modulos.Autenticacao.Dominio.Enums;

namespace Modulos.Autenticacao.Aplicacao.CasosDeUso.CriarUsuario;

public class CriarUsuarioHandler : IRequestHandler<CriarUsuarioCommand>
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;

    public CriarUsuarioHandler(IUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
    }

    public async Task Handle(CriarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var senhaHash = HashSenha(request.Senha);
        var usuario = new Usuario(request.Nome, request.Email, senhaHash, Enum.Parse<Papel>(request.Papel));

        await _usuarioRepositorio.AdicionarAsync(usuario);
    }

    //TODO: Mover para um serviço de hash de senha e talvez implementar um algoritmo mais seguro no futuro
    private string HashSenha(string senha)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: senha!, salt: salt, prf: KeyDerivationPrf.HMACSHA256, iterationCount: 100000, numBytesRequested: 256 / 8));
        
        return $"{hashed}-{Convert.ToBase64String(salt)}";
    }
}