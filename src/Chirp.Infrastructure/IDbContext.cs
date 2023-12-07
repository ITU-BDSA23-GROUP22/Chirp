namespace Chirp.Infrastructure
{
    public interface IDbContext
    {
        Task SaveChanges();
    }
}