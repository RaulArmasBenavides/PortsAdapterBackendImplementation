using CompanyName.DDD.Domain.Common.Log;
using CompanyName.DDD.Domain.Common.Struct;
using CompanyName.DDD.Domain.ProductStockAggregate.Application.DTO;
using CompanyName.DDD.Domain.ProductStockAggregate.Application.Interface;
using CompanyName.DDD.Domain.SaleAggregate.Application.DTO;
using CompanyName.DDD.Domain.SaleAggregate.Application.Interface;
using CompanyName.DDD.Domain.SaleAggregate.Domain;
using System.ComponentModel;
namespace CompanyName.DDD.Domain.SaleAggregate.Application
{
    public class SaleAggregate(ISaleRepository repository, IProductStockAggregate productStock, ILog log) : ISaleAggregate
    {
        private readonly Guid processId = Guid.NewGuid();
        private readonly string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
        public SaleCreateResponse Create(SaleCommand command, string creationUser)
        {
            log.Send(new BodyLog(processId, environment, LogLevelStruct.Info, "Inicio de venta", command));
            var sale = new Sale(command.subtotal, command.taxAmount, command.total, creationUser);
            command.saleDetails.ForEach(detail =>
            {
                var newDetail = sale.AddDetail(detail.productCode, detail.price, detail.quantity, detail.subtotal, detail.taxAmount, detail.total);
                if (!newDetail.ValidateTotal(detail.taxPercentage))
                {
                    var message = $"El total del producto {detail.productName} no es correcto";
                    log.Send(new BodyLog(processId, environment, LogLevelStruct.Error, message, detail));
                    throw new WarningException(message);
                }
                productStock.CreateOrUpdate(new ProductStockCommand(detail.productCode, detail.quantity), processId);
                log.Send(new BodyLog(processId, environment, LogLevelStruct.Info, "Nuevo detalle", detail));
            });
            if (!sale.ValidateTotal())
            {
                var message = "El total de la venta no coincide con el total de los detaller";
                log.Send(new BodyLog(processId, environment, LogLevelStruct.Error, message, command));
                throw new WarningException(message);
            }
            repository.Create(sale);
            repository.SaveChanges();
            return new SaleCreateResponse(sale.SaleCode.ToString());
        }
    }
}