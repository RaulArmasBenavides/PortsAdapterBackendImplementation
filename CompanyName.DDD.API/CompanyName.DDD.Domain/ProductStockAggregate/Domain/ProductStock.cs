using CompanyName.DDD.Domain.Common.Audit;

namespace CompanyName.DDD.Domain.ProductStockAggregate.Domain
{
    public class ProductStock : AuditableEntity
    {
        public Guid ProductStockCode { get; private set; }
        public Guid ProductCode { get; private set; }
        public int Stock { get; private set; }
        public ProductStock(){}
        public ProductStock(Guid productCode, int stock)
        {
            ProductStockCode = Guid.NewGuid();
            ProductCode = productCode;
            Stock = stock;
        }
        public void IncrementStock(int stock)
        {
            Stock += stock;
        }
    }
}