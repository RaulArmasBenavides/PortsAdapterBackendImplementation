using CompanyName.DDD.Domain.ProductAggregate.Domain;
using Microsoft.EntityFrameworkCore;
namespace CompanyName.DDD.Domain.Common.EFCore.Configuration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product").HasKey("ProductCode");
        }
    }
}