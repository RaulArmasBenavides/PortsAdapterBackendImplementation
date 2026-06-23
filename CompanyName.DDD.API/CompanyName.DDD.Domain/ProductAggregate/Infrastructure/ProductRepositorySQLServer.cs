using CompanyName.DDD.Domain.Common.EFCore;
using CompanyName.DDD.Domain.ProductAggregate.Application.DTO;
using CompanyName.DDD.Domain.ProductAggregate.Application.Interfaces;
using CompanyName.DDD.Domain.ProductAggregate.Domain;
using CompanyName.DDD.Domain.TaxTypeAggregate.Domain;
using System.Linq.Expressions;
namespace CompanyName.DDD.Domain.ProductAggregate.Infrastructure
{
    public class ProductRepositorySQLServer(Context context) : ProductRepository
    {
        public List<ProductSimpleResponse> GetBySaleFilter(string productName, string taxTypeName)
        {
            var containsMethodInfo = typeof(string).GetMethod("Contains", [typeof(string)]);
            #region ProductClauses
            var productParameters = Expression.Parameter(typeof(Product), "product");
            //Prodiedades 
            var productNameProperty = Expression.Property(productParameters, "ProductName");
            var productActiveProperty = Expression.Property(productParameters, "Active");
            //Clausulas
            var materialStateClause = Expression.Equal(productActiveProperty, Expression.Constant(true));
            var productNameClause = Expression.Call(productNameProperty, containsMethodInfo, Expression.Constant(productName.Trim()));
            //where
            Expression<Func<Product, bool>> productWhere;
            BinaryExpression productExpression = materialStateClause;
            if (productName != "-") productExpression = Expression.AndAlso(productExpression, productNameClause);
            productWhere = Expression.Lambda<Func<Product, bool>>(productExpression, productParameters);
            #endregion
            #region TaxTypeClauses
            var taxTypeParameters = Expression.Parameter(typeof(TaxType), "taxType");
            //Prpiedades
            var taxTypeNameProperty = Expression.Property(taxTypeParameters, "TaxTypeName");
            //Clausulas
            var taxTypeNameClause = Expression.Call(taxTypeNameProperty, containsMethodInfo, Expression.Constant(taxTypeName.Trim()));
            //Where
            Expression<Func<TaxType, bool>> taxTypeWhere;
            BinaryExpression taxTypeExpression = Expression.Equal(Expression.Constant("1"), Expression.Constant("1"));
            if (taxTypeName != "-") taxTypeExpression = Expression.AndAlso(taxTypeExpression, taxTypeNameClause);
            taxTypeWhere = Expression.Lambda<Func<TaxType, bool>>(taxTypeExpression, taxTypeParameters);
            #endregion
            var q = from product in context.Products.Where(productWhere)
                    join taxType in context.TaxTypes.Where(taxTypeWhere) on product.TaxTypeCode equals taxType.TaxTypeCode
                    select new ProductSimpleResponse(product.ProductCode, product.ProductName, product.Price, product.TaxTypeCode,
                    taxType.TaxTypeName, taxType.TaxPercentage);
            var list = q.ToList();
            return list;
        }
    }
}