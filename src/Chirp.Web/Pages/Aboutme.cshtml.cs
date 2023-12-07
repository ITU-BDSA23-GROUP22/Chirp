using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Core.Services;

namespace Chirp.Pages
{
    public class AboutMeModel : PageModel
    {

        public AuthorDTO? Author { get; set; } = null;

        public async Task<ActionResult> OnPost()
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            Author = await chirpService.GetAuthor(User.Claims?.SingleOrDefault(x => x.Type == "emails")?.Value ?? string.Empty);
            return Page();
        }

        private readonly IChirpService chirpService;

        public IEnumerable<CheepDTO> Cheeps { get; set; } = null!;

        public AboutMeModel(IChirpService chirpService)
        {
            this.chirpService = chirpService;
        }
    }
}
