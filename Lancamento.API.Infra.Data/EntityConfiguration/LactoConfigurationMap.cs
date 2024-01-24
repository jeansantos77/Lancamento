using Lancamento.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lancamento.API.Infra.Data.EntityConfiguration
{
    public class LactoConfigurationMap : IEntityTypeConfiguration<Lacto>
    {
        public void Configure(EntityTypeBuilder<Lacto> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Descricao)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(c => c.Tipo)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(1);

            builder.Property(c => c.Valor)
                   .IsRequired();

            builder.HasOne(c => c.Usuario)
                   .WithMany()
                   .HasForeignKey(c => c.UsuarioId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired();
        }
    }
}
