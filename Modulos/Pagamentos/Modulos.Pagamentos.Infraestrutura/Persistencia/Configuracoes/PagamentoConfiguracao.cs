using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modulos.Pagamentos.Dominio.Entidades;
using Modulos.Pagamentos.Dominio.Enums;

namespace Modulos.Pagamentos.Infraestrutura.Persistencia.Configuracoes;

internal class PagamentoConfiguracao : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.PedidoId)
               .IsRequired();

        builder.Property(p => p.Valor)
               .IsRequired();

        builder.Property(p => p.Status)
               .HasConversion<string>()
               .IsRequired();

        builder.Property(p => p.CriadoEm)
               .IsRequired();

        builder.HasIndex(p => p.PedidoId)
               .IsUnique()
               .HasFilter($"\"{nameof(Pagamento.Status)}\" = '{nameof(StatusPagamento.Aprovado)}'");
    }
}
