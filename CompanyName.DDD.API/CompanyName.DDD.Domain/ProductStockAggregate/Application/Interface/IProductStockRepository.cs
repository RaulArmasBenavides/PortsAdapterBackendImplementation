using CompanyName.DDD.Domain.ProductStockAggregate.Domain;
namespace CompanyName.DDD.Domain.ProductStockAggregate.Application.Interface
{
    public interface IProductStockRepository
    {
        void Create(ProductStock productStock);
        void Update(ProductStock productStock);
        ProductStock? Find(Guid productCode);
        void SaveChanges();
    }
}