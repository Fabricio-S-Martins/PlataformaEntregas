using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modulos.Pedidos.Dominio.Entidades;

namespace Modulos.Pedidos.Infraestrutura.Persistencia.Configuracoes
{
    internal class PedidoConfiguracao : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ClienteId)
                   .IsRequired();

            builder.Property(p => p.RestauranteId)
                   .IsRequired();

            builder.Property(p => p.CriadoEm)
                   .IsRequired();

            builder.Ignore(p => p.Itens);

            builder.HasMany<ItemPedido>("ItensInterno")
                   .WithOne()
                   .HasForeignKey(i => i.PedidoId);
        }
    }
}