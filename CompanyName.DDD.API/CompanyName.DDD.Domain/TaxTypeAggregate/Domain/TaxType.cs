namespace CompanyName.DDD.Domain.TaxTypeAggregate.Domain
{
    public class TaxType
    {
        public int TaxTypeCode { get; private set; }
        public string TaxTypeName { get; private set; }
        public decimal TaxPercentage { get; private set; }
        public TaxType() { }
    }
}