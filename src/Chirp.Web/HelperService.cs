using Chirp.Core.Services;

namespace Chirp.Web
{
    public interface IHelperService
    {
        Task<Guid> getAuthorId();
    }

    public class HelperService : IHelperService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IChirpService chirpService;

        public HelperService(IHttpContextAccessor httpContextAccessor, IChirpService chirpService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.chirpService = chirpService;
        }

        public async Task<Guid> getAuthorId()
        {
            var email = this.httpContextAccessor.HttpContext?.User.Claims?.SingleOrDefault(x => x.Type == "emails")?.Value ?? string.Empty; 
            var authorDto = await this.chirpService.GetAuthor(email);

            if (authorDto == null)
            {
                return Guid.Empty;
            }

            return authorDto.Id;
        }
    }
}