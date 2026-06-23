using CompanyName.DDD.Domain.ProductAggregate.Application.DTO;
namespace CompanyName.DDD.Domain.ProductAggregate.Application.Interfaces
{
    public interface ProductRepository
    {
        List<ProductSimpleResponse> GetBySaleFilter(string productName, string taxDescriprion );
    }
}