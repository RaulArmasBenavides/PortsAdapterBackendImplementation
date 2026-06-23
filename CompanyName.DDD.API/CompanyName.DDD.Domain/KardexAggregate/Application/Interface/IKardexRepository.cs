using CompanyName.DDD.Domain.KardexAggregate.Domain;
namespace CompanyName.DDD.Domain.KardexAggregate.Application.Interface
{
    public interface IKardexRepository
    {
        void Create(Kardex command);
    }
}