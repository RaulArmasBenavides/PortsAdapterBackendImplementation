using CompanyName.DDD.Domain.Common.Cache;
using CompanyName.DDD.Domain.TaxTypeAggregate.Application.DTO;
using CompanyName.DDD.Domain.TaxTypeAggregate.Application.Interface;
namespace CompanyName.DDD.Domain.TaxTypeAggregate.Application
{
    public class TaxTypeAggregate(ITaxTypeRepository repository, IGenericCache genericCache) : ITaxTypeAggregate
    {
        public List<TaxTypeSimpleResponse> GetSimpleResponse()
        {
            var response = genericCache.Get<List<TaxTypeSimpleResponse>>("TaxTypeSimpleResponses");
            if (response == null)
            {
                response = repository.GetSimpleResponse();
                genericCache.Set(response, "TaxTypeSimpleResponses");
            }
            return response;
        }
    }
}