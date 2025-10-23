using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EmpresaX.POS.Infrastructure.Data;

namespace EmpresaX.POS.Infrastructure.Data
{
    public class EmpresaXContextFactory : IDesignTimeDbContextFactory<EmpresaXContext>
    {
        public EmpresaXContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EmpresaXContext>();
            optionsBuilder.UseNpgsql("Host=empresax-pg-21160.postgres.database.azure.com;Port=5432;Database=empresaxposdb;Username=caroladmin;Password=Carol2025!Test;Ssl Mode=Require;Trust Server Certificate=true");
            return new EmpresaXContext(optionsBuilder.Options);
        }
    }
}
