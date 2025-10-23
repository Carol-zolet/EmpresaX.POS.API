using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmpresaX.POS.Domain.Vendas;
using EmpresaX.POS.Domain.Shared.ValueObjects;

namespace EmpresaX.POS.Infrastructure.Data.Configurations
{
    public class VendaConfiguration : IEntityTypeConfiguration<Venda>
    {
        public void Configure(EntityTypeBuilder<Venda> builder)
        {
            builder.ToTable("Vendas");
            builder.HasKey(v => v.Id);

            // Propriedades básicas
            builder.Property(v => v.Data)
                .IsRequired();

            builder.Property(v => v.Descricao)
                .HasMaxLength(200)
                .IsRequired();

            // Value Object Money
            builder.Property(v => v.Valor)
                .HasConversion(
                    money => money.Valor,
                    valor => new Money(valor, "BRL"))
                .HasColumnName("Valor")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // Enums
            builder.Property(v => v.FormaPagamento)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(v => v.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(v => v.Observacoes)
                .HasMaxLength(500);

            // Relacionamento com Cliente (opcional)
            builder.Property(v => v.ClienteId);

            builder.HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.SetNull);

            // Propriedades de auditoria
            builder.Property(v => v.DataCriacao)
                .IsRequired();

            builder.Property(v => v.DataAtualizacao);

            // Índices para consultas de relatórios
            builder.HasIndex(v => v.Data)
                .HasDatabaseName("IX_Vendas_Data");

            builder.HasIndex(v => v.Status)
                .HasDatabaseName("IX_Vendas_Status");

            builder.HasIndex(v => v.FormaPagamento)
                .HasDatabaseName("IX_Vendas_FormaPagamento");

            builder.HasIndex(v => v.ClienteId)
                .HasDatabaseName("IX_Vendas_ClienteId");

            // Índice composto para relatórios por período
            builder.HasIndex(v => new { v.Data, v.Status })
                .HasDatabaseName("IX_Vendas_Data_Status");
        }
    }
}
