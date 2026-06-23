using CompanyName.DDD.Domain.ProductStockAggregate.Application.DTO;
namespace CompanyName.DDD.Domain.ProductStockAggregate.Application.Interface
{
    public interface IProductStockAggregate
    {
        void CreateOrUpdate(ProductStockCommand command, Guid processId);
    }
}