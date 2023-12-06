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
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(nameof(name));
            }
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException(nameof(email));
            }


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
            if (authorId == Guid.Empty)
            {
                throw new ArgumentException(nameof(authorId));
            }

            var author = await dbContext.Authors.SingleOrDefaultAsync(b => b.AuthorId == authorId);
            return author;
        }

        public async Task<Author?> Get(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException(nameof(email));
            }

            var author = await dbContext.Authors.SingleOrDefaultAsync(b => b.Email == email);
            return author;
        }
    }
}