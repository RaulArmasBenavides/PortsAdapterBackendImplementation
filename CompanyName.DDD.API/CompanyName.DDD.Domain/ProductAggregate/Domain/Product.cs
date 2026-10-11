using CompanyName.DDD.Domain.Common.Audit;

namespace CompanyName.DDD.Domain.ProductAggregate.Domain
{
    public class Product : AuditableEntity
    {
        public Guid ProductCode { get; private set; }
        public string ProductName { get; private set; }
        public decimal Price { get; private set; }
        public int TaxTypeCode { get; private set; }
        public bool Active { get; private set; }
        public Product() { }
    }
}