using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modulos.Catalogo.Dominio.Entidades;

namespace Modulos.Catalogo.Infraestrutura.Persistencia.Configuracoes
{
    internal class RestauranteConfiguracao : IEntityTypeConfiguration<Restaurante>
    {
        public void Configure(EntityTypeBuilder<Restaurante> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Nome)
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(r => r.Cnpj)
                   .HasMaxLength(14)
                   .IsRequired();
        }
    }
}
