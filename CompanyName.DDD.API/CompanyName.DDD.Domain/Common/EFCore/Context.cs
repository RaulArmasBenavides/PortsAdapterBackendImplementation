using CompanyName.DDD.Domain.Common.Audit;
using CompanyName.DDD.Domain.Common.EFCore.Configuration;
using CompanyName.DDD.Domain.KardexAggregate.Domain;
using CompanyName.DDD.Domain.ProductAggregate.Domain;
using CompanyName.DDD.Domain.ProductStockAggregate.Domain;
using CompanyName.DDD.Domain.SaleAggregate.Domain;
using CompanyName.DDD.Domain.TaxTypeAggregate.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace CompanyName.DDD.Domain.Common.EFCore
{
    // currentUserService is optional so design-time tools can create the context without DI.
    public class Context(DbContextOptions<Context> options, ICurrentUserService? currentUserService = null) : DbContext(options)
    {
        public DbSet<TaxType> TaxTypes { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<Kardex> Kardexs { get; set; }
        public DbSet<Product> Products { get; set; }
        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInformation()
        {
            var utcNow = DateTime.UtcNow;
            var userName = currentUserService?.UserName;

            foreach (EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = utcNow;
                        entry.Entity.CreatedBy = userName;
                        break;

                    case EntityState.Modified:
                        // Creation data is never overwritten by an update.
                        entry.Property(e => e.CreatedAt).IsModified = false;
                        entry.Property(e => e.CreatedBy).IsModified = false;
                        entry.Entity.UpdatedAt = utcNow;
                        entry.Entity.UpdatedBy = userName;
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaxTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SaleConfiguration());
            modelBuilder.ApplyConfiguration(new SaleDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ProductStockConfiguration());
            modelBuilder.ApplyConfiguration(new KardexConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
    }
}