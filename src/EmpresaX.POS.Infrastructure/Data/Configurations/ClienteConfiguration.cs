using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmpresaX.POS.Domain.CRM;
using EmpresaX.POS.Domain.Shared.ValueObjects;

namespace EmpresaX.POS.Infrastructure.Data.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");
            builder.HasKey(c => c.Id);

            // Value Objects como tipos próprios (owned types)
            builder.OwnsOne(c => c.Nome, nb =>
            {
                nb.Property(n => n.PrimeiroNome)
                    .HasColumnName("PrimeiroNome")
                    .HasMaxLength(60)
                    .IsRequired();

                nb.Property(n => n.Sobrenome)
                    .HasColumnName("Sobrenome")
                    .HasMaxLength(60)
                    .IsRequired();
            });

            builder.OwnsOne(c => c.Email, eb =>
            {
                eb.Property(e => e.Endereco)
                    .HasColumnName("Email")
                    .HasMaxLength(150)
                    .IsRequired();

                eb.HasIndex(e => e.Endereco)
                    .IsUnique()
                    .HasDatabaseName("IX_Clientes_Email");
            });

            builder.OwnsOne(c => c.Telefone, tb =>
            {
                tb.Property(t => t.Numero)
                    .HasColumnName("Telefone")
                    .HasMaxLength(20);
            });

            builder.Property(c => c.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(c => c.Observacoes)
                .HasMaxLength(500);

            builder.Property(c => c.DataCriacao)
                .IsRequired();

            builder.Property(c => c.DataAtualizacao);

            // Índices
            builder.HasIndex(c => c.Status)
                .HasDatabaseName("IX_Clientes_Status");
        }
    }
}
