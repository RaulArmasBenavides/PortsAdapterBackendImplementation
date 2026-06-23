using CompanyName.DDD.Domain.Common.EFCore;
using CompanyName.DDD.Domain.TaxTypeAggregate.Application.DTO;
using CompanyName.DDD.Domain.TaxTypeAggregate.Application.Interface;
namespace CompanyName.DDD.Domain.TaxTypeAggregate.Infrastructure.SQLServer
{
    public class TaxTypeRepository(Context context) : ITaxTypeRepository
    {
        public List<TaxTypeSimpleResponse> GetSimpleResponse()
        {
            var list = (from taxType in context.TaxTypes
                        select new TaxTypeSimpleResponse(taxType.TaxTypeCode, taxType.TaxTypeName, taxType.TaxPercentage)
                        ).ToList();
            return list;
        }
    }
}