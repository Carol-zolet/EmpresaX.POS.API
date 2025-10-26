using Microsoft.EntityFrameworkCore;
using SistemaCaixa.Domain.Entities;

namespace SistemaCaixa.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Caixa> Caixas => Set<Caixa>();
    public DbSet<Movimento> Movimentos => Set<Movimento>();
    public DbSet<Conciliacao> Conciliacoes => Set<Conciliacao>();
    public DbSet<LancamentoBancario> LancamentosBancarios => Set<LancamentoBancario>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações customizadas (Fluent API)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
