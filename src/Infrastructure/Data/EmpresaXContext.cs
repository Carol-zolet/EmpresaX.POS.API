using Microsoft.EntityFrameworkCore;
using EmpresaX.POS.Domain.Entities;

namespace EmpresaX.POS.Infrastructure.Data
{
    public class EmpresaXContext : DbContext
    {
        public EmpresaXContext(DbContextOptions<EmpresaXContext> options) : base(options)
        {
        }

        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<ContaPagar> ContasPagar { get; set; }
        public DbSet<ContaReceber> ContasReceber { get; set; }
        public DbSet<CentroCusto> CentrosCusto { get; set; }
        public DbSet<ContaBancaria> ContasBancarias { get; set; }
        public DbSet<MovimentacaoBancaria> MovimentacoesBancarias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venda> Vendas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações de relacionamentos
            modelBuilder.Entity<ContaPagar>()
                .HasOne(cp => cp.Fornecedor)
                .WithMany(f => f.ContasPagar)
                .HasForeignKey(cp => cp.FornecedorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ContaPagar>()
                .HasOne(cp => cp.CentroCusto)
                .WithMany(cc => cc.ContasPagar)
                .HasForeignKey(cp => cp.CentroCustoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ContaReceber>()
                .HasOne(cr => cr.Cliente)
                .WithMany()
                .HasForeignKey(cr => cr.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimentacaoBancaria>()
                .HasOne(mb => mb.ContaBancaria)
                .WithMany(cb => cb.Movimentacoes)
                .HasForeignKey(mb => mb.ContaBancariaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices para performance
            modelBuilder.Entity<ContaPagar>()
                .HasIndex(cp => cp.DataVencimento);

            modelBuilder.Entity<ContaPagar>()
                .HasIndex(cp => cp.Status);

            modelBuilder.Entity<ContaReceber>()
                .HasIndex(cr => cr.DataVencimento);

            modelBuilder.Entity<MovimentacaoBancaria>()
                .HasIndex(mb => mb.Data);

            modelBuilder.Entity<MovimentacaoBancaria>()
                .HasIndex(mb => mb.Conciliado);
        }
    }
}


