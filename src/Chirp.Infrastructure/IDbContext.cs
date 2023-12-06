namespace Chirp.Infrastructure
{
    /// <summary>
    /// This interface allows us to hide the actual DbContext implementation
    /// to our service, as the service only need to coordinate saving changes
    /// </summary>
	public interface IDbContext
    {
        Task SaveChanges();
    }
}