using Microsoft.EntityFrameworkCore;
using Modulos.Pedidos.Dominio.Entidades;

namespace Modulos.Pedidos.Infraestrutura.Persistencia
{
    public class PedidosDbContext : DbContext
    {
        public DbSet<Pedido> Pedidos { get; set; }
        public PedidosDbContext(DbContextOptions<PedidosDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidosDbContext).Assembly);
        }
    }
}