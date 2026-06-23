using CompanyName.DDD.Domain.SaleAggregate.Domain;
namespace CompanyName.DDD.Test.SaleTest
{
    [TestClass]
    public class SaleTest
    {
        [TestMethod]
        public async Task TestValidateTotalDetailFalse()
        {
            var igvPercent = 0.18m;
            var price = 2.33m;
            var quantity = 5;
            var subtotal = 200;
            var taxAmount = 200;
            var total = 1000;
            var saleDetail = new SaleDetail(Guid.NewGuid(), Guid.NewGuid(), price, quantity, subtotal, taxAmount, total);
            var response = saleDetail.ValidateTotal(igvPercent);
            Assert.IsFalse(response);
        }
        [TestMethod]
        public async Task TestValidateTotalDetailTrue()
        {
            var igvPercent = 0.18m;
            var price = 2.33m;
            var quantity = 5;
            var subtotal = price * quantity;
            var taxAmount = subtotal * igvPercent;
            var total = subtotal + taxAmount;
            var saleDetail = new SaleDetail(Guid.NewGuid(), Guid.NewGuid(), price, quantity, subtotal, taxAmount, total);
            var response = saleDetail.ValidateTotal(igvPercent);
            Assert.IsTrue(response);
        }
        [TestMethod]
        public async Task TestValidateTotalSaleFalse()
        {
            var igvPercent = 0.18m;
            var subtotal = 100;
            var taxAmount = 200;
            var total = subtotal + taxAmount;
            var sale = new Sale(subtotal, taxAmount, total, "");
            //First Product            
            var price1 = 2.33m;
            var quantity1 = 5;
            var subtotal1 = price1 * (quantity1 * 2);
            var taxAmount1 = subtotal1 * igvPercent;
            var total1 = subtotal1 + taxAmount1;
            sale.AddDetail(Guid.NewGuid(), price1, quantity1, subtotal1, taxAmount1, total1);
            //Second producto
            var price2 = 3.11m;
            var quantity2 = 2;
            var subtotal2 = price2 * quantity2;
            var taxAmount2 = subtotal2 * igvPercent;
            var total2 = subtotal2 + taxAmount2;
            sale.AddDetail(Guid.NewGuid(), price2, quantity2, subtotal2, taxAmount2, total2);
            var response = sale.ValidateTotal();
            Assert.IsFalse(response);
        }
        [TestMethod]
        public async Task TestValidateTotalSaleTrue()
        {
            var igvPercent = 0.18m;
            //First Product            
            var price1 = 2.33m;
            var quantity1 = 5;
            var subtotal1 = price1 * quantity1;
            var taxAmount1 = subtotal1 * igvPercent;
            var total1 = subtotal1 + taxAmount1;
            //Second producto
            var price2 = 3.11m;
            var quantity2 = 2;
            var subtotal2 = price2 * quantity2;
            var taxAmount2 = subtotal2 * igvPercent;
            var total2 = subtotal2 + taxAmount2;
            //Sale
            var subtotal = subtotal1 + subtotal2;
            var taxAmount = taxAmount1 + taxAmount2;
            var total = total1 + total2;
            var sale = new Sale(subtotal, taxAmount, total, "");
            sale.AddDetail(Guid.NewGuid(), price1, quantity1, subtotal1, taxAmount1, total1);
            sale.AddDetail(Guid.NewGuid(), price2, quantity2, subtotal2, taxAmount2, total2);
            var response = sale.ValidateTotal();
            Assert.IsTrue(response);
        }
    }
}