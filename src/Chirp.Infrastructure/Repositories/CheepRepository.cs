using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure
{
    public class CheepRepository : ICheepRepository
    {
        readonly ChirpDBContext db;
        public CheepRepository(ChirpDBContext chirpContext)
        {
            db = chirpContext;
        }

        public async Task<Cheep> Create(Author author, string text, DateTime timestamp)
        {
            var cheep = new Cheep { Text = text, TimeStamp = timestamp, AuthorId = author.AuthorId, Author = author };

            await db.Cheeps.AddAsync(cheep);

            return cheep;
        }

        public async Task<IEnumerable<Cheep>> GetAll(int page)
        {
            var cheeps = await db.Cheeps
                .OrderByDescending(c => c.TimeStamp)
                .Skip(32 * (page - 1))
                .Take(32)
                .ToListAsync();

            return cheeps;
        }

        public async Task<IEnumerable<Cheep>> GetByAuthor(Author author, int page)
        {
            var cheeps = await db.Cheeps
                .OrderByDescending(c => c.TimeStamp)
                .Where(cheep => cheep.Author.AuthorId == author.AuthorId)
                .Skip(32 * (page - 1))
                .Take(32)
                .ToListAsync();

            return cheeps;
        }
    }
}