using Microsoft.EntityFrameworkCore;
using Modulos.Catalogo.Dominio.Entidades;

namespace Modulos.Catalogo.Infraestrutura.Persistencia
{
    public class CatalogoDbContext : DbContext
    {
        public DbSet<Restaurante> Restaurantes { get; set; }
        public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoDbContext).Assembly);
        }
    }
}