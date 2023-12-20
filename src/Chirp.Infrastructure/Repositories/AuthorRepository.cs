using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ChirpDBContext dbContext;

        public AuthorRepository(ChirpDBContext chirpDbContext)
        {
            dbContext = chirpDbContext ?? throw new ArgumentNullException(nameof(chirpDbContext));
        }

        public async Task<Author> Create(Guid authorId, string name)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentException(nameof(name));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(nameof(name));
            }

            if (dbContext.Authors.Any(a => a.AuthorId == authorId))
            {
                throw new Exception($"Failed to create author - an author with authorId already exists");
            }

            var author = new Author
            {
                AuthorId = authorId,
                Name = name,
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

        public async Task<AuthorAuthorRelation> FollowAuthor(Author author, Author authorToFollow, DateTime timeStamp)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            if (authorToFollow == null)
            {
                throw new ArgumentNullException(nameof(authorToFollow));
            }

            if (author.AuthorId == authorToFollow.AuthorId)
            {
                throw new Exception($"Failed to FollowAuthor - Author cannot follow self");
            }

            if (author.Following.Any(x => x.AuthorToFollowId == authorToFollow.AuthorId))
            {
                throw new Exception($"Failed to FollowAuthor - Already following Author");
            }

            var authorAuthorRelation = new AuthorAuthorRelation
            {
                Author = author,
                AuthorId = author.AuthorId,
                AuthorToFollow = authorToFollow,
                AuthorToFollowId = authorToFollow.AuthorId,
                TimeStamp = timeStamp,
            };

            await dbContext.AuthorAuthorRelations.AddAsync(authorAuthorRelation);

            return authorAuthorRelation;
        }

        public async Task UnfollowAuthor(Author author, Author authorToUnFollow)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            if (authorToUnFollow == null)
            {
                throw new ArgumentNullException(nameof(authorToUnFollow));
            }

            if (author.AuthorId == authorToUnFollow.AuthorId)
            {
                throw new Exception($"Failed to UnfollowAuthor - Author cannot unfollow self");
            }

            if (!author.Following.Any(x => x.AuthorToFollowId == authorToUnFollow.AuthorId))
            {
                throw new Exception($"Failed to UnfollowAuthor - Author is not following Author");
            }

            var authorAuthorRelation = await dbContext.AuthorAuthorRelations.SingleOrDefaultAsync(x =>
                x.AuthorId == author.AuthorId &&
                x.AuthorToFollowId == authorToUnFollow.AuthorId);

            if (authorAuthorRelation == null)
            {
                throw new Exception($"Failed to UnfollowAuthor - Relation not found");
            }

            dbContext.AuthorAuthorRelations.Remove(authorAuthorRelation);
        }

        public async Task DeleteAuthor(Author author) {
            dbContext.Remove(author);
        }
    }
}