using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ChirpDBContext dbContext;

        public AuthorRepository(ChirpDBContext chirpDbContext)
        {
            this.dbContext = chirpDbContext;
        }

        public async Task<Author> Create(string name, string email)
        {
            if (dbContext.Authors.Any(a => a.Email == email))
            {
                throw new Exception($"Failed to create author - an author with email [{email}] already exists");
            }

            var author = new Author
            {
                Name = name,
                Email = email,
                Cheeps = new List<Cheep>()
            };

            await dbContext.Authors.AddAsync(author);

            return author;
        }

        public async Task<Author?> Get(Guid authorId)
        {
            var author = await dbContext.Authors.SingleOrDefaultAsync(b => b.AuthorId == authorId);
            return author;
        }

        public async Task<Author?> Get(string email)
        {
            var author = await dbContext.Authors.SingleOrDefaultAsync(b => b.Email == email);
            return author;
        }

        public async Task<Author> AddFollowing(Author author, Author following)
        {
            await dbContext.Authors.Where(dbAuthor => dbAuthor.AuthorId == author.AuthorId).AddAsync(following);
            // OR add following dbSet on ChirpDBContext?
            // await dbContext.Following....
        }

        public async Task<IEnumerable<Author?>> GetFollowing(Author author)
        {
            var following = await db.Authors
                .Where(cheep => cheep.Author.AuthorId == author.AuthorId)
                .ToListAsync();

            return following;
        }
    }
}