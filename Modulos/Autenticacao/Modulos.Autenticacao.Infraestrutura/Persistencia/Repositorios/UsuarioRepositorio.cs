using Microsoft.EntityFrameworkCore;
using Modulos.Autenticacao.Aplicacao.Repositorios;
using Modulos.Autenticacao.Dominio.Entidades;

namespace Modulos.Autenticacao.Infraestrutura.Persistencia.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private AutenticacaoDbContext _context;
        public UsuarioRepositorio(AutenticacaoDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            await _context.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario> ObterPorEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Valor == email);
        }
    }
}
