using Lancamento.API.Domain.Entities;
using Lancamento.API.Infra.Data.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Lancamento.API.Infra.Data.Context
{
    public class LancamentoContext : DbContext
    {
        public LancamentoContext(DbContextOptions<LancamentoContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Lacto> Lancamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioConfigurationMap());
            modelBuilder.ApplyConfiguration(new LactoConfigurationMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
