namespace Chirp.Infrastructure
{
    /// <summary>
    ///     Provides means to isolate SaveCanges for Services 
    /// </summary>
    public interface IDbContext
    {
        Task SaveChanges();
    }
}