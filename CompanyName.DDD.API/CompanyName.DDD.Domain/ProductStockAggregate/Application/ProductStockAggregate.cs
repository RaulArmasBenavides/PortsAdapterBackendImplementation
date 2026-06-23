using CompanyName.DDD.Domain.Common.Log;
using CompanyName.DDD.Domain.Common.Struct;
using CompanyName.DDD.Domain.KardexAggregate.Application.DTO;
using CompanyName.DDD.Domain.KardexAggregate.Application.Interface;
using CompanyName.DDD.Domain.ProductStockAggregate.Application.DTO;
using CompanyName.DDD.Domain.ProductStockAggregate.Application.Interface;
using CompanyName.DDD.Domain.ProductStockAggregate.Domain;
namespace CompanyName.DDD.Domain.ProdProductStockAggregateuctStock.Application
{
    public class ProductStockAggregate(IProductStockRepository repository, IKardexAggregate kardex, ILog log) : IProductStockAggregate
    {
        private readonly string _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
        public void CreateOrUpdate(ProductStockCommand command, Guid processId)
        {
            log.Send(new BodyLog(processId, _environment, LogLevelStruct.Info, "Inicio de movimiento de stock", command));
            var productStock = repository.Find(command.productCode);
            var saleQuantity = command.quantity * -1;
            int currentStock = 0;
            if (productStock == null)
            {
                productStock = new ProductStock(command.productCode, saleQuantity);
                repository.Create(productStock);
                log.Send(new BodyLog(processId, _environment, LogLevelStruct.Info, "Registado de nuevo movimieno de stock", productStock));
            }
            else
            {
                currentStock = productStock.Stock;
                productStock.IncrementStock(saleQuantity);
                repository.Update(productStock);
                log.Send(new BodyLog(processId, _environment, LogLevelStruct.Info, "Actualizado movimieno de stock", productStock));
            }
            kardex.Create(new KardexCommand(command.productCode, currentStock, saleQuantity, productStock.Stock), processId);
        }
    }
}