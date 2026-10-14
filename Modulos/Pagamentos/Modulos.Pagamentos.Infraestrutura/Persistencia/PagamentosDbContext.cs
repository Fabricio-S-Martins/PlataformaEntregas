using Microsoft.EntityFrameworkCore;
using Modulos.Pagamentos.Dominio.Entidades;

namespace Modulos.Pagamentos.Infraestrutura.Persistencia;

public class PagamentosDbContext : DbContext
{
    public DbSet<Pagamento> Pagamentos { get; set; }
    public PagamentosDbContext(DbContextOptions<PagamentosDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentosDbContext).Assembly);
    }
}
