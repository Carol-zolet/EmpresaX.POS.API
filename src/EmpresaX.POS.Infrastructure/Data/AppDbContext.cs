using Microsoft.EntityFrameworkCore;
using EmpresaX.POS.Domain.Entities;
using EmpresaX.POS.Domain.CRM;
using EmpresaX.POS.Domain.Vendas;

namespace EmpresaX.POS.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Caixa> Caixas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura a entidade Cliente e seus Value Objects
            modelBuilder.Entity<Cliente>(cliente =>
            {
                // Mapeia o Value Object 'Nome' para colunas na tabela Clientes
                cliente.OwnsOne(c => c.Nome, nome =>
                {
                    nome.Property(n => n.PrimeiroNome).HasColumnName("Nome");
                    nome.Property(n => n.Sobrenome).HasColumnName("Sobrenome");
                });

                // Mapeia o Value Object 'Email'
                cliente.OwnsOne(c => c.Email, email =>
                {
                    email.Property(e => e.Endereco).HasColumnName("Email");
                });

                // Mapeia o Value Object 'Telefone'
                cliente.OwnsOne(c => c.Telefone, telefone =>
                {
                    telefone.Property(t => t.Numero).HasColumnName("Telefone");
                });

                // Converte o enum 'Status' para uma string no banco de dados
                cliente.Property(c => c.Status)
                       .HasConversion<string>();
            });
        }
    }
}