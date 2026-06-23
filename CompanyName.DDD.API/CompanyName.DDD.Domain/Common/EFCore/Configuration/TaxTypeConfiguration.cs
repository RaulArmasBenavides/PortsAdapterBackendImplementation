using CompanyName.DDD.Domain.TaxTypeAggregate.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CompanyName.DDD.Domain.Common.EFCore.Configuration
{
    public class TaxTypeConfiguration : IEntityTypeConfiguration<TaxType>
    {
        public void Configure(EntityTypeBuilder<TaxType> builder)
        {
            builder.ToTable("TaxType").HasKey(x => x.TaxTypeCode);
        }
    }
}