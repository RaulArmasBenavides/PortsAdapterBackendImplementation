using CompanyName.DDD.Domain.Common.EFCore;
using CompanyName.DDD.Domain.ProductStockAggregate.Application.Interface;
using CompanyName.DDD.Domain.ProductStockAggregate.Domain;
namespace CompanyName.DDD.Domain.ProductStockAggregate.Infrrastrucure.SQLServer
{
    public class ProductStockRepository(Context context) : IProductStockRepository
    {
        public void Create(ProductStock productStock)
        {
            context.ProductStocks.Add(productStock);
        }        
        public void Update(ProductStock productStock)
        {
            context.ProductStocks.Update(productStock);
        }        
        public ProductStock? Find(Guid productCode)
        {
            var row = context.ProductStocks.Where(x => x.ProductCode.Equals(productCode)).SingleOrDefault();
            return row;
        }
        public void SaveChanges() => context.SaveChanges();
    }
}