namespace CompanyName.DDD.API.Extensions
{
    public static class OptionsExtension
    {
        public static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<CompanyInformationConfiguration>(configuration.GetSection("CompanyInformation"));
        }
    }
}