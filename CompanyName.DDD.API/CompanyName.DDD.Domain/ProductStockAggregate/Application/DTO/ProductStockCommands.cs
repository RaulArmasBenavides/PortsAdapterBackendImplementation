namespace CompanyName.DDD.Domain.ProductStockAggregate.Application.DTO
{
    public record ProductStockCommand(Guid productCode, int quantity);
}