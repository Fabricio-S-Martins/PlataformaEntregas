using Microsoft.EntityFrameworkCore;
using Modulos.Autenticacao.Aplicacao.Repositorios;
using Modulos.Autenticacao.Dominio.Entidades;

namespace Modulos.Autenticacao.Infraestrutura.Persistencia.Repositorios;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private AutenticacaoDbContext Context;
    public UsuarioRepositorio(AutenticacaoDbContext context)
    {
        Context = context;
    }

    public async Task AdicionarAsync(Usuario usuario)
    {
        await Context.AddAsync(usuario);
        await Context.SaveChangesAsync();
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        return await Context.Usuarios.FirstOrDefaultAsync(u => u.Email.Valor == email);
    }
}