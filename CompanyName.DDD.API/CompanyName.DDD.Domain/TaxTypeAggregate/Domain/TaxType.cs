using CompanyName.DDD.Domain.Common.Audit;

namespace CompanyName.DDD.Domain.TaxTypeAggregate.Domain
{
    public class TaxType : AuditableEntity
    {
        public int TaxTypeCode { get; private set; }
        public string TaxTypeName { get; private set; }
        public decimal TaxPercentage { get; private set; }
        public TaxType() { }
    }
}