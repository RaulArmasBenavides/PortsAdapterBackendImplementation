namespace CompanyName.DDD.Domain.ProductAggregate.Application.DTO
{
    public record ProductSimpleResponse(Guid ProductCode, string ProductName, decimal Price, int TaxTypeCode, string TaxTypeName, decimal TaxPercentage);
}
