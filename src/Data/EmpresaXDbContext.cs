using Microsoft.EntityFrameworkCore;

namespace EmpresaX.POS.API.Data
{
    public class EmpresaXDbContext : DbContext
    {
        public EmpresaXDbContext(DbContextOptions<EmpresaXDbContext> options) : base(options) { }

        public DbSet<ContaPagarEntity> ContasPagar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContaPagarEntity>(entity =>
            {
                entity.ToTable("ContasPagar");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Fornecedor).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Descricao).HasMaxLength(500);
                entity.Property(e => e.Valor).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Vencimento).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Categoria).HasMaxLength(100);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
            });
        }
    }

    public class ContaPagarEntity
    {
        public int Id { get; set; }
        public string Fornecedor { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime Vencimento { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int DiasAtraso { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
