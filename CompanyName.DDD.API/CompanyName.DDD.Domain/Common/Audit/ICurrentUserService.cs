namespace CompanyName.DDD.Domain.Common.Audit
{
    /// <summary>
    /// Exposes the user performing the current request, used to fill audit fields.
    /// </summary>
    public interface ICurrentUserService
    {
        string? UserName { get; }
    }
}
