// The service to communicate with front-end, instead of the frontend communicating directly with the repositories
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
            this.cheepRepository = cheepRepository;
            this.authorRepository = authorRepository;
            this.dbContext = dbContext;
        }

        public async Task<AuthorDTO> GetAuthor(Guid authorId)
        {
            var author = await this.authorRepository.Get(authorId)
                ?? throw new Exception($"Failed to get author - Author not found for id [{authorId}]");

            return this.MapAuthorToDto(author);
        }

        public async Task CreateCheep(AuthorDTO authorDto, string text)
        {
            var author = await this.authorRepository.Get(authorDto.Email);

            if (author == null)
            {
                author = await this.authorRepository.Create(authorDto.Name, authorDto.Email);
            }

            var timestamp = DateTime.Now;
            await this.cheepRepository.Create(author, text, timestamp);

            await dbContext.SaveChanges();
        }

        public async Task<IEnumerable<CheepDTO>> GetAllCheeps(int page)
        {
            var cheeps = await this.cheepRepository.GetAll(page);

            var result = new List<CheepDTO>();
            foreach (var cheep in cheeps)
            {
                var authorDto = await this.GetAuthor(cheep.AuthorId)
                    ?? throw new Exception($"Failed to get author - Author not found for id [{cheep.AuthorId}]");

                result.Add(this.MapCheepToDto(cheep, authorDto));
            }
            return result;
        }

        public async Task<IEnumerable<CheepDTO>> GetCheepsByAuthor(Guid authorId, int page)
        {
            var author = await this.authorRepository.Get(authorId)
                ?? throw new Exception($"Failed to get author - Author not found for id [{authorId}]");

            var cheeps = await this.cheepRepository.GetByAuthor(author, page);

            var authorDto = this.MapAuthorToDto(author);
            return cheeps.Select(cheep => this.MapCheepToDto(cheep, authorDto)).ToList();
        }


        #region Private methods

        private CheepDTO MapCheepToDto(Cheep cheep, AuthorDTO authorDto)
        {
            return new CheepDTO(
                authorDto,
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
                author.Email
                );
        }

        #endregion
    }
}