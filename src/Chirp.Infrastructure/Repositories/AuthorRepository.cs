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
                Cheeps = new List<Cheep>(),
                Following = new List<AuthorAuthorRelation>()
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

            var author = await dbContext.Authors
                .Include(x => x.Following)
                .SingleOrDefaultAsync(b => b.AuthorId == authorId);

            return author;
        }

        public async Task<Author?> Get(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException(nameof(email));
            }

            var author = await dbContext.Authors
                .Include(x => x.Following)
                .SingleOrDefaultAsync(b => b.Email == email);

            return author;
        }

        public async Task<AuthorAuthorRelation> FollowAuthor(Author author, Author authorToFollow)
        {
            // Add validation and guard

            var authorAuthorRelation = new AuthorAuthorRelation
            {
                Author = author,
                AuthorId = author.AuthorId,
                AuthorToFollow = authorToFollow,
                AuthorToFollowId = authorToFollow.AuthorId,
                TimeStamp = DateTime.UtcNow
            };

            await this.dbContext.AuthorAuthorRelations.AddAsync(authorAuthorRelation);

            return authorAuthorRelation;
        }

        public async Task UnfollowAuthor(Author author, Author authorToUnfollow)
        {
            var authorAuthorRelation = await this.dbContext.AuthorAuthorRelations.SingleAsync(x =>
                x.AuthorId == author.AuthorId &&
                x.AuthorToFollowId == authorToUnfollow.AuthorId);


            this.dbContext.AuthorAuthorRelations.Remove(authorAuthorRelation);

        }

        //public async Task<IEnumerable<Author?>> GetFollowing(Author author)
        //{
        //    var following = await db.Authors
        //        .Where(cheep => cheep.Author.AuthorId == author.AuthorId)
        //        .ToListAsync();

        //    return following;
        //}
    }
}