// ReferenceLink:
//  https://learn.microsoft.com/en-us/ef/core/
//  https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application

using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure
{
    /// <inheritdoc/>
    public class CheepRepository : ICheepRepository
    {
        private readonly ChirpDBContext dbContext;

        public CheepRepository(ChirpDBContext chirpDbContext)
        {
            dbContext = chirpDbContext ?? throw new ArgumentNullException(nameof(chirpDbContext));
        }

        /// <inheritdoc/>
        public async Task<Cheep> Create(Author author, string text, DateTime timestamp)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException(nameof(text));
            }

            var cheep = new Cheep { Text = text, TimeStamp = timestamp, AuthorId = author.AuthorId, Author = author };

            await dbContext.Cheeps.AddAsync(cheep);

            return cheep;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Cheep>> GetAll(int page, int skipCount, int takeCount)
        {
            if (skipCount < 0)
            {
                throw new ArgumentException(nameof(skipCount));
            }
            if (takeCount < 1)
            {
                throw new ArgumentException(nameof(takeCount));
            }
            var cheeps = await dbContext.Cheeps
                .Include(x => x.Author)
                .OrderByDescending(c => c.TimeStamp)
                .Skip(skipCount)
                .Take(takeCount)
                .ToListAsync();

            return cheeps;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Cheep>> GetByAuthors(IEnumerable<Guid> authorIds, int page, int skipCount, int takeCount)
        {
            if (skipCount < 0)
            {
                throw new ArgumentException(nameof(skipCount));
            }
            if (takeCount < 1)
            {
                throw new ArgumentException(nameof(takeCount));
            }
            if (authorIds == null)
            {
                throw new ArgumentNullException(nameof(authorIds));
            }

            if (!authorIds.Any())
            {
                throw new Exception("Failed to GetByAuthors - AuthorIds empty");
            }

            var cheeps = await dbContext.Cheeps
                .Include(x => x.Author)
                .Where(cheep => authorIds.Contains(cheep.Author.AuthorId))
                .OrderByDescending(c => c.TimeStamp)
                .Skip(skipCount)
                .Take(takeCount)
                .ToListAsync();

            return cheeps;
        }

        /// <inheritdoc/>
        public async Task AnonymizeCheeps(Author author)
        {
            var cheeps = await dbContext.Cheeps
                .Include(x => x.Author)
                .Where(a => a.Author == author)
                .ToListAsync();

            Author anonymizedAuthor = new Author { Name = "" };

            foreach(Cheep loopCheep in cheeps)
            {
                loopCheep.Author = anonymizedAuthor;
            }
        }
    }
}