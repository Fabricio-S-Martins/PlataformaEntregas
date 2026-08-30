using Microsoft.EntityFrameworkCore;
using Modulos.Autenticacao.Dominio.Entidades;

namespace Modulos.Autenticacao.Infraestrutura.Persistencia;

public class AutenticacaoDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public AutenticacaoDbContext(DbContextOptions<AutenticacaoDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutenticacaoDbContext).Assembly);
    }
}