using CompanyName.DDD.Domain.Common.EFCore;
using CompanyName.DDD.Domain.KardexAggregate.Application.Interface;
using CompanyName.DDD.Domain.KardexAggregate.Domain;
namespace CompanyName.DDD.Domain.KardexAggregate.Infrastructure.SQLServer
{
    public class KardexRepository(Context context) : IKardexRepository
    {
        public void Create(Kardex command)
        {
            context.Kardexs.Add(command);
        }
    }
}