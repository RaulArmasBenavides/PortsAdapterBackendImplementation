namespace CompanyName.DDD.Domain.SaleAggregate.Domain
{
    public class SaleDetail
    {
        public Guid SaleCode { get; private set; }
        public Guid SaleDetailCode { get; private set; }
        public Guid ProductCode { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public decimal Subtotal { get; private set; }
        public decimal TaxAmount { get; private set; }
        public decimal Total { get; private set; }
        public virtual Sale Sale { get; private set; }
        public SaleDetail() { }
        public SaleDetail(Guid saleCode, Guid productCode, decimal price, int quantity, decimal subtotal, decimal taxAmount, decimal total)
        {
            SaleDetailCode = Guid.NewGuid();
            SaleCode = saleCode;
            ProductCode = productCode;
            Price = price;
            Quantity = quantity;
            Subtotal = subtotal;
            TaxAmount = taxAmount;
            Total = total;
        }
        public bool ValidateTotal(decimal igvPercent)
        {
            var subtotal = Math.Round(Quantity * Price, 5);
            var taxAmount = Math.Round(subtotal * igvPercent, 5);
            var total = Math.Round(subtotal + taxAmount, 5);
            return total == Total;
        }
    }
}