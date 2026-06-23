namespace CompanyName.DDD.Domain.KardexAggregate.Application.DTO
{
    public record KardexCommand(Guid productCode, int initialStock, int quantity, int finalStock);
}