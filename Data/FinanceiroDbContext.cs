using Microsoft.EntityFrameworkCore;
using EmpresaX.POS.API.Models;

namespace EmpresaX.POS.API.Data
{
    public class FinanceiroDbContext : DbContext
    {
        public FinanceiroDbContext(DbContextOptions<FinanceiroDbContext> options) : base(options)
        {
        }

        public DbSet<FechamentoCaixa> FechamentosCaixa { get; set; }
        public DbSet<MovimentacaoCaixa> MovimentacoesCaixa { get; set; }
        public DbSet<ConciliacaoBancaria> ConciliacoesBancarias { get; set; }
        public DbSet<ItemConciliacao> ItensConciliacao { get; set; }
    }
}
