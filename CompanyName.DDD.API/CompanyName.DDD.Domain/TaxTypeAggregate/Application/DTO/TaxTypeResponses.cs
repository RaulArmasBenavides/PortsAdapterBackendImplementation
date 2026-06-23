namespace CompanyName.DDD.Domain.TaxTypeAggregate.Application.DTO
{
    public record TaxTypeSimpleResponse(int TaxTypeCode, string TaxTypeName, decimal TaxPercentage);
}