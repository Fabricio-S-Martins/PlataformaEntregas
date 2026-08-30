using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modulos.Autenticacao.Dominio.Entidades;

namespace Modulos.Autenticacao.Infraestrutura.Persistencia.Configuracoes;

public class UsuarioConfiguracao : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nome)
               .HasMaxLength(150)
               .IsRequired();

        builder.OwnsOne(u => u.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Valor)
                        .HasColumnName(nameof(Usuario.Email))
                        .HasMaxLength(150)
                        .IsRequired();
        });

        builder.Property(u => u.SenhaHash)
               .HasMaxLength(400)
               .IsRequired();

        builder.Property(u => u.Papel)
               .HasConversion<string>()
               .HasMaxLength(50)
               .IsRequired();
    }
}