using Chirp.Core;
using Chirp.Core.Services;

namespace Chirp.Infrastructure.Services
{
    public class ChirpService : IChirpService
    {
        private readonly ICheepRepository cheepRepository;
        private readonly IAuthorRepository authorRepository;
        private readonly IDbContext dbContext;

        public ChirpService(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IDbContext dbContext)
        {
            this.cheepRepository = cheepRepository ?? throw new ArgumentNullException(nameof(cheepRepository));
            this.authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<AuthorDTO?> GetAuthor(Guid authorId)
        {
            var author = await this.authorRepository.Get(authorId);

            if (author == null)
            {
                return null;
            }

            return this.MapAuthorToDto(author);
        }

        public async Task<AuthorDTO> CreateAuthor(Guid authorId, string authorName)
        {
            var author = await this.authorRepository.Get(authorId);

            if (author != null)
            {
                throw new Exception("Failed to CreateAuthor - Author already exists with id");
            }

            author = await this.authorRepository.Create(authorId, authorName);

            await this.dbContext.SaveChanges();

            return this.MapAuthorToDto(author);
        }

        public async Task<IEnumerable<CheepDTO>> GetAllCheeps(int page, int skipCount, int takeCount)
        {
            var cheeps = await this.cheepRepository.GetAll(page, skipCount, takeCount);

            var result = new List<CheepDTO>();
            foreach (var cheep in cheeps)
            {
                var authorDto = await this.GetAuthor(cheep.AuthorId)
                    ?? throw new Exception($"Failed to get author - Author not found for id [{cheep.AuthorId}]");

                result.Add(this.MapCheepToDto(cheep, authorDto));
            }
            return result;
        }

        public async Task<IEnumerable<CheepDTO>> GetCheepsByAuthors(IEnumerable<Guid> authorIds, int page, int skipCount, int takeCount)
        {
            if (authorIds == null)
            {
                throw new ArgumentNullException(nameof(authorIds));
            }

            if (!authorIds.Any())
            {
                throw new Exception("Failed to GetCheepsByAuthors - AuthorIds empty");
            }
            var cheeps = await this.cheepRepository.GetByAuthors(authorIds, page, skipCount, takeCount);

            var authorDtos = cheeps
                .DistinctBy(x => x.Author)
                .Select(x => this.MapAuthorToDto(x.Author)).ToArray();

            return cheeps.Select(cheep => this.MapCheepToDto(cheep, authorDtos)).ToList();
        }

        public async Task CreateCheep(Guid authorId, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException(nameof(text));
            }

            var author = await this.authorRepository.Get(authorId)
                ?? throw new Exception($"Failed to CreateCheep - Author not found");

            var timestamp = DateTime.Now;
            await this.cheepRepository.Create(author, text, timestamp);

            await dbContext.SaveChanges();
        }

        public async Task FollowAuthor(Guid authorId, Guid authorToFollowId)
        {
            var author = await this.authorRepository.Get(authorId)
                ?? throw new Exception($"Failed to FollowAuthor - Author not found");

            var authorToFollow = await this.authorRepository.Get(authorToFollowId)
                ?? throw new Exception($"Failed to FollowAuthor - Author not found");

            var timeStamp = DateTime.Now;
            await this.authorRepository.FollowAuthor(author, authorToFollow, timeStamp);

            await dbContext.SaveChanges();
        }

        public async Task UnfollowAuthor(Guid authorId, Guid authorToUnfollowId)
        {
            var author = await this.authorRepository.Get(authorId)
                ?? throw new Exception($"Failed to FollowAuthor - Author not found");

            var authorToUnfollow = await this.authorRepository.Get(authorToUnfollowId)
                ?? throw new Exception($"Failed to UnfollowAuthor - Author not found");

            await this.authorRepository.UnfollowAuthor(author, authorToUnfollow);

            await dbContext.SaveChanges();
        }

        public async Task<IEnumerable<AuthorDTO>> SearchAuthors(string? searchText, int page, int skipCount, int takeCount)
        {
            var authors = await this.authorRepository.SearchAuthor(searchText, page, skipCount, takeCount);

            return authors.Select(x => MapAuthorToDto(x));

        }

        private CheepDTO MapCheepToDto(Cheep cheep, AuthorDTO authorDto)
        {
            return MapCheepToDto(cheep, new[] { authorDto });
        }

        private CheepDTO MapCheepToDto(Cheep cheep, IEnumerable<AuthorDTO> authorDtos)
        {
            return new CheepDTO(
                authorDtos.SingleOrDefault(x => x.Id == cheep.AuthorId) ?? throw new Exception("Author not selected"),
                cheep.CheepId,
                cheep.Text,
                cheep.TimeStamp.ToString()
                );
        }

        private AuthorDTO MapAuthorToDto(Author author)
        {
            return new AuthorDTO(
                author.AuthorId,
                author.Name,
                author.Following?.Select(x => x.AuthorToFollowId).ToArray() ?? Enumerable.Empty<Guid>()
                );
        }
    }
}