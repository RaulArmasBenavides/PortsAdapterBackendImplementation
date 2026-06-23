namespace CompanyName.DDD.Domain.KardexAggregate.Domain
{
    public class Kardex
    {
        public Guid KardexCode { get; private set; }
        public DateTime KardexDate { get; private set; }
        public Guid ProductCode { get; private set; }
        public int InitialStock { get; private set; }
        public int Quantity { get; private set; }
        public int FinalStock { get; private set; }
        public string Symbol { get; private set; }
        public Kardex() { }
        public Kardex(Guid productCode, int initialStock, int quantity, int finalStock, string symbol)
        {
            KardexCode = Guid.NewGuid();
            KardexDate = DateTime.Now;
            ProductCode = productCode;
            InitialStock = initialStock;
            Quantity = quantity;
            FinalStock = finalStock;
            Symbol = symbol;
        }
    }
}