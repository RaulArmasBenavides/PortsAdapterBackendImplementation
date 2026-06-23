using CompanyName.DDD.Domain.SaleAggregate.Domain;
namespace CompanyName.DDD.Domain.SaleAggregate.Application.Interface
{
    public interface ISaleRepository
    {
        void Create(Sale sale);
        void SaveChanges();
    }
}