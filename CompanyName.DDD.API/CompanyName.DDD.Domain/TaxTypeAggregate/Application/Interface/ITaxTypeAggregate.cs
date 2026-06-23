using CompanyName.DDD.Domain.TaxTypeAggregate.Application.DTO;
namespace CompanyName.DDD.Domain.TaxTypeAggregate.Application.Interface
{
    public interface ITaxTypeAggregate
    {
        List<TaxTypeSimpleResponse> GetSimpleResponse();
    }
}