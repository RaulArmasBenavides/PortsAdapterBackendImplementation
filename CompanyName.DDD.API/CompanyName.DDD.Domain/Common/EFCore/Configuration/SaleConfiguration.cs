using CompanyName.DDD.Domain.SaleAggregate.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CompanyName.DDD.Domain.Common.EFCore.Configuration
{
    internal class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sale").HasKey(x => x.SaleCode);
            builder.HasMany(x => x.SaleDetails).WithOne(x => x.Sale).HasForeignKey(x => x.SaleCode);
        }
    }
    internal class SaleDetailConfiguration : IEntityTypeConfiguration<SaleDetail>
    {
        public void Configure(EntityTypeBuilder<SaleDetail> builder)
        {
            builder.ToTable("SaleDetail").HasKey(x => x.SaleDetailCode);
        }
    }
}