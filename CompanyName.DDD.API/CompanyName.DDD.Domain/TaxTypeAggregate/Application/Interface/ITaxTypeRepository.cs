using CompanyName.DDD.Domain.TaxTypeAggregate.Application.DTO;
namespace CompanyName.DDD.Domain.TaxTypeAggregate.Application.Interface
{
    public interface ITaxTypeRepository
    {
        List<TaxTypeSimpleResponse> GetSimpleResponse();
    }
}