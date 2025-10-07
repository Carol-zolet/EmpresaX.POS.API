using Microsoft.EntityFrameworkCore;
using EmpresaX.POS.API.Domain.Entities;

namespace EmpresaX.POS.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura a propriedade 'Metadata' da entidade 'Produto'
            // para ser mapeada para uma única coluna do tipo JSONB no PostgreSQL.
            modelBuilder.Entity<Produto>()
                .OwnsOne(p => p.Metadata, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.ToJson();
                });
        }
    }
}