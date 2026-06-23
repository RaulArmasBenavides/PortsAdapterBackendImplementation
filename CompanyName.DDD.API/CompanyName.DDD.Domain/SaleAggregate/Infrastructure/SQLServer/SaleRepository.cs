using CompanyName.DDD.Domain.Common.EFCore;
using CompanyName.DDD.Domain.SaleAggregate.Application.Interface;
using CompanyName.DDD.Domain.SaleAggregate.Domain;
namespace CompanyName.DDD.Domain.SaleAggregate.Infrastructure.SQLServer
{
    public class SaleRepository(Context context) : ISaleRepository
    {
        public void Create(Sale sale)
        {
            context.Sales.Add(sale);
        }
        public void SaveChanges() => context.SaveChanges();
    }
}