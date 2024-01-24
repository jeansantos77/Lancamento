using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Lancamento.API.Infra.Data.Context
{
    public class LancamentoContextFactory : IDesignTimeDbContextFactory<LancamentoContext>
    {
        public LancamentoContext CreateDbContext(string[] args)
        {
            return CreateDbContext("DefaultConnection");
        }

        public LancamentoContext CreateDbContext(string connectionStringName)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<LancamentoContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(connectionStringName));

            return new LancamentoContext(optionsBuilder.Options);
        }


    }
}
