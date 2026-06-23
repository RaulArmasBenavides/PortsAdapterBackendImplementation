using CompanyName.DDD.Domain.KardexAggregate.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CompanyName.DDD.Domain.Common.EFCore.Configuration
{
    internal class KardexConfiguration : IEntityTypeConfiguration<Kardex>
    {
        public void Configure(EntityTypeBuilder<Kardex> builder)
        {
            builder.ToTable("Kardex").HasKey(x => x.KardexCode); 
        }
    }
}