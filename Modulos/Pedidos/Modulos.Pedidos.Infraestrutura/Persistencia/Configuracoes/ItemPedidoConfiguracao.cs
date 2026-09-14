using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modulos.Pedidos.Dominio.Entidades;

namespace Modulos.Pedidos.Infraestrutura.Persistencia.Configuracoes
{
    internal class ItemPedidoConfiguracao : IEntityTypeConfiguration<ItemPedido>
    {
        public void Configure(EntityTypeBuilder<ItemPedido> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.PedidoId)
                   .IsRequired();

            builder.Property(i => i.ItemCardapioId)
                   .IsRequired();

            builder.Property(i => i.Quantidade)
                   .IsRequired();

            builder.Property(i => i.PrecoUnitario)
                   .IsRequired();
        }
    }
}