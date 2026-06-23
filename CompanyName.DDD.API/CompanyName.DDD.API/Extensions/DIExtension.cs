using CompanyName.DDD.Domain.Common.Cache;
using CompanyName.DDD.Domain.Common.Cache.Implementation;
using CompanyName.DDD.Domain.Common.Log;
using CompanyName.DDD.Domain.Common.Log.LogImplementations;
using CompanyName.DDD.Domain.KardexAggregate.Application;
using CompanyName.DDD.Domain.KardexAggregate.Application.Interface;
using CompanyName.DDD.Domain.KardexAggregate.Infrastructure.SQLServer;
using CompanyName.DDD.Domain.ProdProductStockAggregateuctStock.Application;
using CompanyName.DDD.Domain.ProductAggregate.Application;
using CompanyName.DDD.Domain.ProductAggregate.Application.Interfaces;
using CompanyName.DDD.Domain.ProductAggregate.Infrastructure;
using CompanyName.DDD.Domain.ProductStockAggregate.Application.Interface;
using CompanyName.DDD.Domain.ProductStockAggregate.Infrrastrucure.SQLServer;
using CompanyName.DDD.Domain.SaleAggregate.Application;
using CompanyName.DDD.Domain.SaleAggregate.Application.Interface;
using CompanyName.DDD.Domain.SaleAggregate.Infrastructure.SQLServer;
using CompanyName.DDD.Domain.TaxTypeAggregate.Application;
using CompanyName.DDD.Domain.TaxTypeAggregate.Application.Interface;
using CompanyName.DDD.Domain.TaxTypeAggregate.Infrastructure.SQLServer;
namespace CompanyName.DDD.API.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services)
        {
            services.AddTransient<ITaxTypeAggregate, TaxTypeAggregate>();
            services.AddTransient<ITaxTypeRepository, TaxTypeRepository>();
            services.AddTransient<ISaleAggregate, SaleAggregate>();
            services.AddTransient<ISaleRepository, SaleRepository>();
            services.AddTransient<IProductStockAggregate, ProductStockAggregate>();
            services.AddTransient<IProductStockRepository, ProductStockRepository>();
            services.AddTransient<IKardexAggregate, KardexAggregate>();
            services.AddTransient<IKardexRepository, KardexRepository>();
            services.AddTransient<IProductAggregate, ProductAggregate>();
            services.AddTransient<ProductRepository, ProductRepositorySQLServer>();
            services.AddTransient<ILog, StandartOuputLog>();
            services.AddTransient<IGenericCache, InMemoryCache>();
            return services;
        }
    }
}