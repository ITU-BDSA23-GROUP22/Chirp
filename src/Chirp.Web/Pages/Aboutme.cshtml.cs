using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Core.Services;
using System.ComponentModel.DataAnnotations;
using Chirp.Web.ViewModels;

namespace Chirp.Web.Pages
{
    public class AboutMeModel : PageModel
    {
        public AuthorDTO? Author { get; set; } = null;

        public async Task<ActionResult> OnPost()
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            return Page();
        }

        private readonly IChirpService chirpService;
        private readonly IPresentationService presentationService;

        public IEnumerable<CheepDTO> Cheeps { get; set; } = null!;
        public AboutMeModel(IChirpService chirpService, IPresentationService presentationService)
        {
            this.presentationService = presentationService
                ?? throw new ArgumentNullException(nameof(presentationService));
            this.chirpService = chirpService;
        }

        public IActionResult OnPostForgetMe()
        {
            Console.WriteLine(presentationService.GetAuthenticatedAuthor());
            chirpService.AnonymizeAuthor(presentationService.GetAuthenticatedAuthor().Id);

            return Page();
        }
    }
}