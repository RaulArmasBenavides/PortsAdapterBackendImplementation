using CompanyName.DDD.Domain.KardexAggregate.Application.DTO;
namespace CompanyName.DDD.Domain.KardexAggregate.Application.Interface
{
    public interface IKardexAggregate
    {
        void Create(KardexCommand command, Guid processId);
    }
}