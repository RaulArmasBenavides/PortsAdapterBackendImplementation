using CompanyName.DDD.Domain.SaleAggregate.Application.DTO;
namespace CompanyName.DDD.Domain.SaleAggregate.Application.Interface
{
    public interface ISaleAggregate
    {
        SaleCreateResponse Create(SaleCommand command, string creationUser);
    }
}