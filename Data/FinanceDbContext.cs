using Microsoft.EntityFrameworkCore;
using EmpresaX.POS.API.Models.Finance;

namespace EmpresaX.POS.API.Data
{
    public class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
        {
        }

        public DbSet<Conta> Contas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }
        public DbSet<FechamentoCaixa> FechamentosCaixa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações da entidade Conta
            modelBuilder.Entity<Conta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.SaldoInicial).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Tipo).HasConversion<string>();
            });

            // Configurações da entidade Categoria
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Tipo).HasConversion<string>();
            });

            // Configurações da entidade Movimentacao
            modelBuilder.Entity<Movimentacao>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Valor).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Tipo).HasConversion<string>();
                
                entity.HasOne(e => e.Conta)
                      .WithMany(c => c.Movimentacoes)
                      .HasForeignKey(e => e.ContaId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Categoria)
                      .WithMany(c => c.Movimentacoes)
                      .HasForeignKey(e => e.CategoriaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurações da entidade FechamentoCaixa
            modelBuilder.Entity<FechamentoCaixa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SaldoInicial).HasColumnType("decimal(18,4)");
                entity.Property(e => e.TotalEntradas).HasColumnType("decimal(18,4)");
                entity.Property(e => e.TotalSaidas).HasColumnType("decimal(18,4)");
                entity.Property(e => e.SaldoFinal).HasColumnType("decimal(18,4)");
                entity.Property(e => e.SaldoInformado).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Diferenca).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Status).HasConversion<string>();

                entity.HasOne(e => e.Conta)
                      .WithMany(c => c.Fechamentos)
                      .HasForeignKey(e => e.ContaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Dados iniciais com datas fixas
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var dataFixa = new DateTime(2025, 9, 13);
            
            // Seed Categorias
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nome = "Vendas", Tipo = TipoCategoria.Receita, Descricao = "Receitas de vendas", DataCriacao = dataFixa, Ativa = true },
                new Categoria { Id = 2, Nome = "Serviços", Tipo = TipoCategoria.Receita, Descricao = "Receitas de serviços", DataCriacao = dataFixa, Ativa = true },
                new Categoria { Id = 3, Nome = "Fornecedores", Tipo = TipoCategoria.Despesa, Descricao = "Pagamentos a fornecedores", DataCriacao = dataFixa, Ativa = true },
                new Categoria { Id = 4, Nome = "Salários", Tipo = TipoCategoria.Despesa, Descricao = "Folha de pagamento", DataCriacao = dataFixa, Ativa = true },
                new Categoria { Id = 5, Nome = "Aluguel", Tipo = TipoCategoria.Despesa, Descricao = "Despesas com aluguel", DataCriacao = dataFixa, Ativa = true }
            );

            // Seed Contas
            modelBuilder.Entity<Conta>().HasData(
                new Conta { Id = 1, Nome = "Caixa Principal", Tipo = TipoConta.Caixa, SaldoInicial = 5000m, DataCriacao = dataFixa, Ativa = true },
                new Conta { Id = 2, Nome = "Banco Santander", Tipo = TipoConta.Banco, SaldoInicial = 15000m, NumeroConta = "123456-7", Agencia = "1234", Banco = "Santander", DataCriacao = dataFixa, Ativa = true },
                new Conta { Id = 3, Nome = "Banco Itaú", Tipo = TipoConta.Banco, SaldoInicial = 8000m, NumeroConta = "987654-3", Agencia = "5678", Banco = "Itaú", DataCriacao = dataFixa, Ativa = true }
            );
        }
    }
}
