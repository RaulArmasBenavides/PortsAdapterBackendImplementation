using CompanyName.DDD.Domain.Common.Cache;
using CompanyName.DDD.Domain.ProductAggregate.Application.DTO;
using CompanyName.DDD.Domain.ProductAggregate.Application.Interfaces;
namespace CompanyName.DDD.Domain.ProductAggregate.Application
{
    public class ProductAggregate(ProductRepository repository, IGenericCache genericCache) : IProductAggregate
    {
        public List<ProductSimpleResponse> GetBySaleFilter(string productName, string taxDescriprion)
        {
            string key = $"{productName}-{taxDescriprion}-GetBySaleFilter";
            var response = genericCache.Get<List<ProductSimpleResponse>>(key);
            if (response == null)
            {
                response = repository.GetBySaleFilter(productName, taxDescriprion);
                genericCache.Set(response, key);
            }
            return response;
        }
    }
}