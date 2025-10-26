using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCaixa.Domain.Entities;

namespace SistemaCaixa.Infrastructure.Data.Configurations;

public class CaixaConfiguration : IEntityTypeConfiguration<Caixa>
{
    public void Configure(EntityTypeBuilder<Caixa> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.OperadorAbertura).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Status).IsRequired();
        builder.OwnsOne(c => c.SaldoInicial);
        builder.OwnsOne(c => c.SaldoFinal);
        builder.HasMany(c => c.Movimentos).WithOne(m => m.Caixa).HasForeignKey(m => m.CaixaId);
    }
}
