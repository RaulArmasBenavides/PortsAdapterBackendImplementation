namespace CompanyName.DDD.Domain.SaleAggregate.Application.DTO
{
    public record SaleCommand(decimal subtotal, decimal taxAmount, decimal total, List<SaleDetailCommand> saleDetails);
    public record SaleDetailCommand(Guid productCode,string productName,  decimal price, decimal taxPercentage, int quantity, decimal subtotal, decimal taxAmount, decimal total);
}