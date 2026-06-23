namespace CompanyName.DDD.Domain.Common.Cache
{
    public interface IGenericCache
    {
        void Set<T>(T row, string key);
        T? Get<T>(string key);
    }
}