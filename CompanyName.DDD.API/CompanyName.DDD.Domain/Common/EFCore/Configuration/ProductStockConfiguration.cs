using CompanyName.DDD.Domain.ProductStockAggregate.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CompanyName.DDD.Domain.Common.EFCore.Configuration
{
    internal class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
    {
        public void Configure(EntityTypeBuilder<ProductStock> builder)
        {
            builder.ToTable("ProductStock").HasKey(x => x.ProductStockCode);
        }
    }
}