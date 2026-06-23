using CompanyName.DDD.Domain.Common.Log;
using CompanyName.DDD.Domain.Common.Struct;
using CompanyName.DDD.Domain.KardexAggregate.Application.DTO;
using CompanyName.DDD.Domain.KardexAggregate.Application.Interface;
using CompanyName.DDD.Domain.KardexAggregate.Domain;
namespace CompanyName.DDD.Domain.KardexAggregate.Application
{
    public class KardexAggregate(IKardexRepository repository, ILog log) : IKardexAggregate
    {
        private readonly string _environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
        public void Create(KardexCommand command, Guid processId)
        {
            log.Send(new BodyLog(processId, _environment, LogLevelStruct.Info, "Inicio de movimiento de kardex", command));
            var symbol = command.quantity < 0 ? "-" : "+";
            var kardex = new Kardex(command.productCode, command.initialStock, command.quantity, command.finalStock, symbol);
            log.Send(new BodyLog(processId, _environment, LogLevelStruct.Info, "Registado nuevo movimiento de kardex", kardex));
            repository.Create(kardex);
        }
    }
}